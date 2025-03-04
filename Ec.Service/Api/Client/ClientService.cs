using Ec.Common.Constants;
using Ec.Common.DtoModels;
using Ec.Common.Models.Client;
using Ec.Common.Models.Otp;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Exceptions;
using Ec.Service.Extentions;
using Ec.Service.Helpers;
using Ec.Service.In_memory_Storage;
using Ec.Service.Otp;
using Newtonsoft.Json;

namespace Ec.Service.Api.Client;

public class ClientService(IUserRepository userRepository,
    RedisService redisService,
    OtpService otpService)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly RedisService _redisService = redisService;
    private readonly OtpService _otpService = otpService;


    public async Task<string> VerifyLogin(OtpModel model)
    {
        try
        {

            Helper.IsValidNumber(model.PhoneNumber);
            await _otpService.AddOtp(model);
            return "successfull";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<int> Login(ClientLoginModel model)
    {
        try
        {
            Helper.IsValidNumber(model.PhoneNumber);
            var client = await IsHaveClient(model.PhoneNumber);
            Helper.VerfyPassword(client, model.Password);
            await _redisService.Set(client.PhoneNumber, client);
            int code = _otpService.GenerateCode(model.PhoneNumber);
            return code;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<UserDto> VerifyRegister(OtpModel model)
    {
        try
        {

            Helper.IsValidNumber(model.PhoneNumber);
            await _otpService.AddOtp(model);
            var redisValue = await _redisService.Get(model.PhoneNumber);
            var client = JsonConvert.DeserializeObject<User>(redisValue);
            await _userRepository.AddAsync(client);
            return client.ParseToDto();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<int> Register(ClientRegisterModel model)
    {
        try
        {
            Helper.IsValidNumber(model.PhoneNumber);
            await CheckClientExistInMemory(model.PhoneNumber);
            await CheckClientExistInDb(model.PhoneNumber);
            var newClient = new User()
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Role = Constants.ClientRole
            };
            var hashedPass = Helper.HashPassword(newClient, model.Password);
            newClient.PasswordHash = hashedPass;
            Helper.UserDtos.Add(newClient.ParseToDto());
            await _redisService.Set(newClient.PhoneNumber, newClient);
            await _redisService.Set(Constants.UserDtos, Helper.UserDtos);
            int code = _otpService.GenerateCode(newClient.PhoneNumber);
            return code;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }


    private async Task CheckClientExistInDb(string phoneNumber)
    {

        var client = await _userRepository.GetUserByPhoneNumber(phoneNumber);
        if (client is not null)
            throw new AccountExist();

    }
    private async Task CheckClientExistInMemory(string phoneNumber)
    {

        var redisValue = await _redisService.Get(phoneNumber);
        if (redisValue.HasValue)
        {
            var client = JsonConvert.DeserializeObject<User>(redisValue);
            if (client is not null)
                throw new AccountExist();
        }


    }
    private async Task<User> IsHaveClient(string phoneNumber)
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            var client = users.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
            if (client is null)
                throw new NoSuchAccountExist();
            return client;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }




    public async Task<UserDto> GetClientAccount(Guid clientId)
    {
        var client = await GetClient(clientId);
        return client.ParseToDto();
    }


    private async Task<User> GetClient(Guid clientId)
    {
        var client = await _userRepository.GetUserById(clientId);
        Helper.CheckClientExist(client);
        Helper.CheckClientRole(client.Role);
        return client;
    }


}