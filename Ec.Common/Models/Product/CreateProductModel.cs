using Ec.Data.Enums;

namespace Ec.Common.Models.Product;

public class CreateProductModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; } = decimal.Zero;

}