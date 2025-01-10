using Ec.Common.Models.Client;
using Ec.Common.Models.Otp;
using Ec.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Ec.Service.Helper;

public static class HelperExtension
{
    public static void IsValidNumber(string phoneNumber)
    {
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


    public static string Check(ClientLoginModel model)
    {
        if (!string.IsNullOrEmpty(model.PhoneNumber))
        {
            IsValidNumber(model.PhoneNumber);
            return model.PhoneNumber;
        }
        else if (!string.IsNullOrEmpty(model.Username))
        {
            return model.Username;
        }
        else
        {
            throw new Exception("Please enter a phone number or username");
        }
    }



    public static string Check(OtpModel model)
    {
        if (!string.IsNullOrEmpty(model.PhoneNumber))
        {
            IsValidNumber(model.PhoneNumber);
            return model.PhoneNumber;
        }
        else if (!string.IsNullOrEmpty(model.Username))
        {
            return model.Username;
        }
        else
        {
            throw new Exception("Please enter a phone number or username");
        }
    }
}
