using Ec.Common.Constants;
using Ec.Common.Models.Otp;
using Ec.Common.Models.Seller;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Helper;
using Ec.Service.In_memory_Storage;
using Ec.Service.MemoryCache;
using Ec.Service.Otp;

namespace Ec.Service.Api;

public class SellerService(IUserRepository userRepository, OtpService otpService, MemoryCacheService memoryCacheService, RedisService redisService)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly OtpService _otpService = otpService;
    private readonly MemoryCacheService _memoryCacheService = memoryCacheService;
    private readonly RedisService _redisService = redisService;


    public async Task<string> VerifyLogin(OtpModel model)
    {
        HelperExtension.IsValidNumber(model.PhoneNumber);
        CheckSellerExist(model.PhoneNumber);  
        await _otpService.AddOtp(model);
        return "Succesfully";
    }


    public async Task<int> Login(SellerLoginModel model)
    {
        try
        {
            HelperExtension.IsValidNumber(model.PhoneNumber);
            User seller = await IsHaveSeller(model.PhoneNumber);
            _memoryCacheService.Set(Constants.SellerKey, seller, Constants.MemoryExpirationTime);
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

            HelperExtension.IsValidNumber(model.PhoneNumber);
            CheckSellerExist(model.PhoneNumber);
            await IsUniquePhoneNum(model.PhoneNumber);
            var newSeller = new User()
            {
                Id = Guid.NewGuid(),
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Role = Constants.SellerRole,
            };
            _redisService.SetUser(newSeller.PhoneNumber, newSeller);
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
            HelperExtension.IsValidNumber(model.PhoneNumber);
            await _otpService.AddOtp(model);
            var seller = _redisService.GetUser(model.PhoneNumber);
            await _userRepository.AddAsync(seller);
            return "Successfull register";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task IsUniquePhoneNum(string phonenumber)
    {
        var users = await _userRepository.GetAllAsync();
        var isUnique = users.Any(x => x.PhoneNumber == phonenumber);

        if (isUnique)
            throw new InvalidDataException("There is an account opened with this number. Please enter another number.");
    }

    private void CheckSellerExist(string phoneNumber)
    {
        var user = _redisService.GetUser(phoneNumber);
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
    //private string Verify()
    //{
    //    var seller = _memoryCacheService.Get<User>(Constants.SellerKey);
    //    if (seller is null)
    //        throw new Exception("Verification failed");
    //    if(seller.PhoneNumber == )
    //}
}