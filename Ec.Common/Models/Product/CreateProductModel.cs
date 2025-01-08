using Ec.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ec.Common.Models.Product;

public class CreateProductModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; } = decimal.Zero;
    public string Category { get; set; }
    public string? VideoUrl { get; set; }
    public ProductStatus Status { get; set; }
}
