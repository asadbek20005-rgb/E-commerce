using Ec.Data.Enums;
using System.ComponentModel.DataAnnotations;
namespace Ec.Data.Entities;

public class Feedback
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage ="Enter comment")]
    public string Comment { get; set; }
    public Rank Rank { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid SellerId { get; set; }
    public Guid ClientId { get; set; }
    public virtual User? Seller { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
}