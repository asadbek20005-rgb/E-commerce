using Ec.Common.Constants;
using Ec.Common.DtoModels;
using Ec.Common.Models.Client;
using Ec.Common.Models.Otp;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Ec.Service.Helpers;
using Ec.Service.In_memory_Storage;
using Ec.Service.Otp;

namespace Ec.Service.Api.Client;

public class ClientService(IUserRepository userRepository, RedisService redisService, OtpService otpService)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly RedisService _redisService = redisService;
    private readonly OtpService _otpService = otpService;

    public async Task<UserDto> GetProfile(Guid userId)
    {
        var users = await _redisService.GetUsersAsync(Constants.GetUsersAsyncCacheKey);
        if (users is not null)
        {
            var clientCache = users.SingleOrDefault(x => x.Id == userId);
            if (clientCache is not null)
                return clientCache.ParseToDto();
        }

        users = await _userRepository.GetAllAsync();
        var client = users.FirstOrDefault(x => x.Id == userId);
        if (client == null)
            throw new ArgumentNullException(nameof(client));
        await _redisService.SetUser(Constants.GetUsersAsyncCacheKey, client);
        return client.ParseToDto();
    }
    public async Task<string> VerifyLogin(OtpModel model)
    {
        _ = Helper.Check(model);
        await _otpService.AddOtp(model);
        return "successfull";
    }
    public async Task<int> Login(ClientLoginModel model)
    {
        string identifier = Helper.Check(model);
        var client = await IsHaveClient(identifier);
        Helper.VerfyPassword(client, model.Password);
        await _redisService.SetUser(identifier, client);
        int code = _otpService.GenerateCode(identifier);
        return code;
    }
    public async Task<string> VerifyRegister(OtpModel model)
    {
        string identifier = Helper.Check(model);
        await _otpService.AddOtp(model);
        User client = await _redisService.GetUser(identifier);
        await _userRepository.AddAsync(client);
        return "successfull";
    }
    public async Task<int> Register(ClientRegisterModel model)
    {
        try
        {

            await Check(model);
            var newClient = new User()
            {
                FullName = model.FullName,
                Username = model.Username,
                PhoneNumber = model.PhoneNumber,
                Role = Constants.ClientRole
            };
            var hashedPass = Helper.HashPassword(newClient, model.Password);
            Caching(newClient);
            int code = GetCode(newClient.PhoneNumber, newClient.Username);
            if (code == 0)
                throw new Exception("code cannot be 0");
            return code;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    private async Task IsUniquePhoneNum(string phonenumber)
    {
        var users = await _userRepository.GetAllAsync();
        var isUnique = users.Any(x => x.PhoneNumber == phonenumber);
        if (isUnique)
            throw new InvalidDataException("There is an account opened with this number. Please enter another number.");
    }
    private async Task IsUniqueUsername(string username)
    {
        var users = await _userRepository.GetAllAsync();
        var isUnique = users.Any(u => u.Username == username);
        if (isUnique)
            throw new InvalidDataException("There is an account opened with this number. Please enter another number.");
    }
    private async void Caching(User user)
    {
        if (!string.IsNullOrEmpty(user.PhoneNumber))
        {
            await _redisService.SetUser(user.PhoneNumber, user);
        }
        else if (!string.IsNullOrEmpty(user.Username))
        {
            await _redisService.SetUser(user.Username, user);
        }
        else
        {
            throw new Exception("Please enter a phone number or username");
        }

    }
    private int GetCode(string? key1, string? key2)
    {
        if (!string.IsNullOrEmpty(key1))
        {
            int code = _otpService.GenerateCode(key1);
            return code;
        }

        if (!string.IsNullOrEmpty(key2))
        {
            int code = _otpService.GenerateCode(key2);
            return code;
        }
        return 0;
    }
    private async Task Check(ClientRegisterModel model)
    {
        if (!string.IsNullOrEmpty(model.PhoneNumber))
        {
            Helper.IsValidNumber(model.PhoneNumber);
            await IsUniquePhoneNum(model.PhoneNumber);
            await CheckClientExist(model);
        }

        if (!string.IsNullOrEmpty(model.Username))
        {
            await IsUniqueUsername(model.Username);

        }
        else
        {

            throw new Exception("Please enter a phone number or username");
        }

    }
    private async Task CheckClientExist(ClientRegisterModel model)
    {
        if (!string.IsNullOrEmpty(model.PhoneNumber))
        {
            User client = await _redisService.GetUser(model.PhoneNumber);
            if (client is not null)
                throw new Exception("An account was opened with this number");
        }

        if (!string.IsNullOrEmpty(model.Username))
        {
            User client = await _redisService.GetUser(model.Username);
            if (client is not null)
                throw new Exception("An account was opened with this username");
        }
    }
    private async Task<User> IsHaveClient(string identifier)
    {
        var users = await _userRepository.GetAllAsync();
        var client = users.FirstOrDefault(x => x.PhoneNumber == identifier || x.Username == identifier);
        if (client is null)
            throw new Exception("no such account exists");
        return client;
    }
}
