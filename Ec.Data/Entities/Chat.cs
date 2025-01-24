using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Chat
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage ="Enter Chat name")]
    public List<string> Names { get; set; }
    
    public List<User_Chat> Users { get; set; }
    public List<Message> Messages { get; set; }

}
