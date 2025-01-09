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


    

}
