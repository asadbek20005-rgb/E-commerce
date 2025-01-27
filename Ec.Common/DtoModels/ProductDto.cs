using Ec.Data.Enums;

namespace Ec.Common.DtoModels;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; } = decimal.Zero;
    public string? VideoUrl { get; set; }
    public Category Category { get; set; }

    public UserDto Seller { get; set; }
}
