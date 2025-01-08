using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Feedback
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage ="Enter comment")]
    public string Comment { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedDate { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

}
