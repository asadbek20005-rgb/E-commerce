using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage ="Enter your full name")]
    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public required string Role { get; set; }
    public int Rank { get; set; }
    public bool IsBlocked { get; set; } = false;
    public DateTime CreatedDate { get; set; }


    public virtual List<Product>? Products { get; set; }
    public virtual List<Feedback>? Feedbacks { get; set; }
    public virtual List<SearchHistory>? SearchHistories { get; set; }
    public virtual List<Complaint>? Complaints { get; set; }
    public virtual List<User_Chat>? Chats { get; set; }
    public virtual Address? Address { get; set; }
    public virtual Statistic? Statistic { get; set; }

}
