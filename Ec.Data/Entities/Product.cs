using Ec.Data.Enums;
using System.ComponentModel.DataAnnotations;
namespace Ec.Data.Entities;

public class Product
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Enter product name")]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required(ErrorMessage ="Enter product price")]
    public decimal Price { get; set; } = decimal.Zero;
    [Required(ErrorMessage = "Enter category")]
    public Category Category { get; set; }
    public int? ViewedCount { get; set; }
    [Required(ErrorMessage = "Enter status")]
    public ProductStatus Status { get; set; }
    public DateOnly CreatedDate { get; set; }


    public Guid SellerId { get; set; }
    public virtual User Seller { get; set; }
    public virtual List<Feedback>? Feedbacks { get; set; }
    public virtual List<ProductContent>? ProductContent { get; set; }
}