using System.ComponentModel.DataAnnotations;

namespace Ec.Common.Models.Seller;

public class SellerRegisterModel
{
    [Required(ErrorMessage = "Please, enter a fullname")]
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}
