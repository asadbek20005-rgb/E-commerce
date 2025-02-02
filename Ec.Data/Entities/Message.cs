using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Message
{
    [Key]
    public int Id { get; set; }
    public string? Text { get; set; }
    [Required]
    public string FromUser { get; set; }
    public Guid FromUserId { get; set; }
    public DateTime SendedAt { get; set; }
    public DateTime EditedAt { get; set; }
    public Guid ChatId { get; set; }
    public virtual Chat Chat { get; set; }
    public virtual MessageContent? Content { get; set; }
}
