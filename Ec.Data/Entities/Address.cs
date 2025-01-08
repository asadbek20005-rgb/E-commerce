using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Address
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage ="Enter Address name")]
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedDate { get; set; }

    public Guid SellerId { get; set; }
    public User Seller { get; set; }
}
