using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Message
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage ="Enter text")]
    public string Text { get; set; }
    public DateTime CreatedDate { get; set; }

    public Guid ChatId { get; set; }
    public Chat Chat { get; set; }
}
