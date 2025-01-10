using Ec.Common.Constants;
using Ec.Common.Models.Otp;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.MemoryCache;

namespace Ec.Service.Otp;

public class OtpService(MemoryCacheService memoryCacheService, IRepository<OTP> repository)
{
    private readonly MemoryCacheService _memoryCacheService = memoryCacheService;
    private readonly IRepository<OTP> _repository = repository;

    public async Task AddOtp(OtpModel model)
    {
        await IsExpired(model.Code);
        CheckValues(model);
        var newOtp = new OTP()
        {
            PhoneNumber = model.PhoneNumber,
            Username = model.Username,
            Code = model.Code,
            IsExpired = true
        };
        await _repository.AddAsync(newOtp);
    }

    public int GenerateCode(string key)
    {
        var random = new Random();
        int code = random.Next(1111, 9999);
        _memoryCacheService.Set(key, code, Constants.MemoryExpirationTime);
        return code;
    }

    public void CheckValues(OtpModel model)
    {
        if (!string.IsNullOrEmpty(model.PhoneNumber))
        {
            int code = _memoryCacheService.GetCode(model.PhoneNumber);
            if (code.Equals(null) || code == 0)
                throw new Exception("There is no code for this number. Enter another number.");

            if (code != model.Code)
                throw new Exception("The code is not valid!");
        }
        else if (!string.IsNullOrEmpty(model.Username))
        {
            int code = _memoryCacheService.GetCode(model.Username);
            if (code.Equals(null) || code == 0)
                throw new Exception("There is no code for this username.");

            if (code != model.Code)
                throw new Exception("The code is not valid!");
        }
        else
        {
            throw new Exception("Please enter phone number or username");
        }


    }


    public async Task IsExpired(int code)
    {
        var otps = await _repository.GetAllAsync();
        if (otps is null || otps.Count == 0)
            return;
        var otp = otps.SingleOrDefault(x => x.Code == code);
        if (otp is null)
            return;
        if (otp.IsExpired)
            throw new Exception("Code is expired");
    }
}
