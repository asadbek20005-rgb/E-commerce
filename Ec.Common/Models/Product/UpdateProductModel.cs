using Ec.Data.Enums;

namespace Ec.Common.Models.Product;

public class UpdateProductModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; } 
    public Category Category { get; set; }
    public ProductStatus Status { get; set; }
}
