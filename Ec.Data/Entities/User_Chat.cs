using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class User_Chat
{
    [Key]
    public Guid Id { get; set; }
    public Guid ToUserId { get; set; }
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
    public virtual User User { get; set; }
    public virtual Chat Chat { get; set; }

}
