using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Chat
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage ="Enter Chat name")]
    public string Name { get; set; }
    
    public List<User_Chat> Users { get; set; }
    public List<Message> Messages { get; set; }

}
