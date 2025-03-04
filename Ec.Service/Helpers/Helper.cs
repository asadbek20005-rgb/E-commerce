using Ec.Common.Constants;
using Ec.Common.DtoModels;
using Ec.Data.Entities;
using Ec.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Ec.Service.Helpers;

public static class Helper
{

    public static void CheckMessageExist(Message message)
    {
        if (message is null) throw new MessageNotFoundException();
    }
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
    public static void CheckPrice(decimal price)
    {
        if (price < 0 || price == 0)
            throw new Exception("Price is null or 0");
    }
    public static async Task<(string FileName, string ContentType, long Size, MemoryStream Data)> SaveFileDetails(IFormFile file)
    {
        var fileName = Guid.NewGuid().ToString();
        string contentType = file.ContentType;
        long size = file.Length;

        var data = new MemoryStream();
        await file.CopyToAsync(data);

        return (fileName, contentType, size, data);
    }
    public static void CheckSellerRole(string role)
    {
        if (role != Constants.SellerRole) throw new Exception("Role Must Be seller");
    }
    public static void CheckAddressForNull(Address addressEntity)
    {
        if (addressEntity is null)
            throw new AddressNotFoundException();
    }
    public static void CheckChatExist(Data.Entities.Chat chat)
    {
        if (chat == null)
            throw new ChatNotFoundException();
    }



    public static void CheckUserExist(User user)
    {
        if (user is null)
            throw new UserNotFoundException();
    }


    public static void CheckClientExist(User client)
    {
        if (client is null)
            throw new ClientNotFoundException();
    }


    public static void CheckClientRole(string role)
    {
        if (role != Constants.ClientRole) throw new Exception("Role must be client");
    }


    public static void CheckSellerExist(User seller)
    {
        if (seller is null)
            throw new SellerNotFoundException();
    }
}
