using Ec.Common.Constants;
using Ec.Common.Models.Client;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Helper;
using Ec.Service.In_memory_Storage;
using Ec.Service.Otp;
using Microsoft.AspNetCore.Identity;

namespace Ec.Service.Api.Client;

public class ClientService(IUserRepository userRepository, RedisService redisService, OtpService otpService)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly RedisService _redisService = redisService;
    private readonly OtpService _otpService = otpService;

    public async Task<int> Register(ClientRegisterModel model)
    {
        await Check(model);
        var newClient = new User()
        {
            FullName = model.FullName,
            Username = model.Username,
            PhoneNumber = model.PhoneNumber,
            Role = Constants.ClientRole
        };
        var hashedPass = HashPassword(newClient, model.Password);
        Caching(newClient);
        int code = GetCode(newClient.PhoneNumber, newClient.Username);
        if (code == 0)
            throw new Exception("code cannot be 0");
        return code;
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

    private string HashPassword(User user, string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException("password");

        var hashedPassword = new PasswordHasher<User>().HashPassword(user, password);
        return hashedPassword;
    }

    private void Caching(User user)
    {
        if (!string.IsNullOrEmpty(user.PhoneNumber))
        {
            _redisService.SetUser(user.PhoneNumber, user);
        }

        if (!string.IsNullOrEmpty(user.Username))
        {
            _redisService.SetUser(user.Username, user);
        }

        throw new Exception("Please enter a phone number or username");
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
            HelperExtension.IsValidNumber(model.PhoneNumber);
            await IsUniquePhoneNum(model.PhoneNumber);
        }
        if (!string.IsNullOrEmpty(model.Username))
            await IsUniqueUsername(model.Username);
        throw new Exception("Please enter a phone number or username");

    }
}
