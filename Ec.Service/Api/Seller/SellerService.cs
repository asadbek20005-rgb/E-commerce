using Ec.Common.Constants;
using Ec.Common.Models.Otp;
using Ec.Common.Models.Seller;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Helpers;
using Ec.Service.In_memory_Storage;
using Ec.Service.Otp;

namespace Ec.Service.Api.Seller;

public class SellerService(IUserRepository userRepository, OtpService otpService, RedisService redisService)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly OtpService _otpService = otpService;
    private readonly RedisService _redisService = redisService;
    public async Task<string> VerifyLogin(OtpModel model)
    {
        Helper.IsValidNumber(model.PhoneNumber);
        IsVerifying(model.PhoneNumber);
        await _otpService.AddOtp(model);
        return "Succesfully";
    }
    public async Task<int> Login(SellerLoginModel model)
    {
        try
        {
            Helper.IsValidNumber(model.PhoneNumber);
            User seller = await IsHaveSeller(model.PhoneNumber);
            if (!string.IsNullOrEmpty(seller.PhoneNumber))
                await _redisService.SetUser(seller.PhoneNumber, seller);
            int code = _otpService.GenerateCode(model.PhoneNumber);
            return code;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<int> Register(SellerRegisterModel model)
    {
        try
        {

            Helper.IsValidNumber(model.PhoneNumber);
            CheckSellerExist(model.PhoneNumber);
            var newSeller = new User()
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Role = Constants.SellerRole,
            };
            await _redisService.SetUser(newSeller.PhoneNumber, newSeller);
            int code = _otpService.GenerateCode(model.PhoneNumber);
            return code;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<string> VerifyRegister(OtpModel model)
    {
        try
        {
            Helper.IsValidNumber(model.PhoneNumber);
            await _otpService.AddOtp(model);
            var seller = await _redisService.GetUser(model.PhoneNumber);
            await _userRepository.AddAsync(seller);
            return "Successfull register";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async void CheckSellerExist(string phoneNumber)
    {
        var user = await _redisService.GetUser(phoneNumber);
        if (user is not null)
            throw new InvalidDataException("There is an account opened with this number. Please enter another number.");
    }
    private async Task<User> IsHaveSeller(string phoneNumber)
    {
        var sellers = await _userRepository.GetAllAsync();
        if (sellers is null || sellers.Count == 0)
            return null;
        var seller = sellers.SingleOrDefault(x => x.PhoneNumber == phoneNumber);
        if (seller == null)
            throw new Exception("No such account exists");
        return seller;
    }
    private async void IsVerifying(string phoneNumber)
    {
        var seller = await _redisService.GetUser(phoneNumber);
        if (seller is null)
            throw new Exception("Verification failed");
    }
}