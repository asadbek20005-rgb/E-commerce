using System.ComponentModel.DataAnnotations;
namespace Ec.Data.Entities;

public class OTP
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage ="Enter phone number")]
    public string PhoneNumber { get; set; }
    public int Code { get; set; }
    public bool IsExpired { get; set; }= false;
}
