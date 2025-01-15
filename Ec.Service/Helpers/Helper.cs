using Ec.Common.DtoModels;
using Ec.Common.Models.Client;
using Ec.Common.Models.Otp;
using Ec.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Ec.Service.Helpers;

public static class Helper
{

    public static List<UserDto>? UserDtos { get; set; } = new List<UserDto>();
    public static List<ProductDto> ProductDtos { get; set; } = new List<ProductDto>();
    public static void IsValidNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            throw new Exception("phone number is null");
        string regix = @"^(\+998|998)?[3-9]\d{1}\d{7}$";
        bool isValid = Regex.IsMatch(phoneNumber, regix);
        if (!isValid)
            throw new InvalidDataException("Please enter a valid number");
    }
    public static string HashPassword(User user, string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        var hashedPassword = new PasswordHasher<User>().HashPassword(user, password);
        return hashedPassword;
    }
    public static void VerfyPassword(User user, string password)
    {
        var verfyPas = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password);
        if (verfyPas == PasswordVerificationResult.Failed)
        {
            throw new Exception($"The validation password is failed!");

        }
    }
    public static string CheckPhoneNumber(string phoneNumber)
    {
        if (!string.IsNullOrEmpty(phoneNumber))
        {
            IsValidNumber(phoneNumber);
            return phoneNumber;
        }
        else
        {
            throw new Exception("Please enter a phone number");
        }
    }
    public static string Check(OtpModel model)
    {
        if (!string.IsNullOrEmpty(model.PhoneNumber))
        {
            IsValidNumber(model.PhoneNumber);
            return model.PhoneNumber;
        }

        throw new Exception("Please enter a phone number");

    }
}
