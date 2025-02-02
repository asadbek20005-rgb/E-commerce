using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class MessageContent
{
    [Key]
    public int Id { get; set; }
    public string FileUrl { get; set; }
    public string? Caption { get; set; }

    public int MessageId { get; set; }
}
