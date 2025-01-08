using Ec.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Product
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage ="Enter product name")]  
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; } = decimal.Zero;
    [Required(ErrorMessage ="Enter category")]
    public required string Category { get; set; }
    public string? VideoUrl { get; set; }
    public int? ViewedCount { get; set; }
    [Required(ErrorMessage ="Enter status")]
    public ProductStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }


    public Guid SellerId { get; set; }
    public User Seller { get; set; }

    public List<Feedback>? Feedbacks { get; set; }

}
