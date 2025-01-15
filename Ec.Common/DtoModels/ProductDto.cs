namespace Ec.Common.DtoModels;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; } = decimal.Zero;
    public required string Category { get; set; }
}
