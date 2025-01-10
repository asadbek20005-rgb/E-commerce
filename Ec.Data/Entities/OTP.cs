using System.ComponentModel.DataAnnotations;
namespace Ec.Data.Entities;

public class OTP
{
    [Key]
    public int Id { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Username { get; set; }
    public int Code { get; set; }
    public bool IsExpired { get; set; }= false;
}
