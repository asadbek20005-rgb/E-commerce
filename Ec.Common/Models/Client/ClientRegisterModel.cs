using System.ComponentModel.DataAnnotations;

namespace Ec.Common.Models.Client;

public class ClientRegisterModel
{
    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Username { get; set; }
    public string Password { get; set; }
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
