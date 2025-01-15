using System.Text.RegularExpressions;

namespace Ec.Common.Models.Client;

public class ClientLoginModel
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}
