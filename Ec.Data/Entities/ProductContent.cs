using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class ProductContent
{
    [Key]
    public int Id { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty; 
    public string? Caption { get; set; }
}
