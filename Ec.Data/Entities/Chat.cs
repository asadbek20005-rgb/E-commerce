using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Chat
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage ="Enter Chat name")]
    public List<string> Names { get; set; }
    
    public virtual List<User_Chat> Users { get; set; }
    public virtual List<Message> Messages { get; set; }

}
