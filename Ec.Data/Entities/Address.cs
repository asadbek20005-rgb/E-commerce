using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Address
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage ="Enter Address name")]
    public string Name { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public DateTime CreatedDate { get; set; }

    public Guid SellerId { get; set; }
    public virtual User Seller { get; set; }
}
