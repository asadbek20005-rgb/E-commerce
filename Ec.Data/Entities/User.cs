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
    [Required(ErrorMessage ="Enter your role")]
    public string Role { get; set; }
    public int Rank { get; set; }
    public bool IsBlocked { get; set; } = false;
    public DateTime CreatedDate { get; set; }


    public List<Product>? Products { get; set; }
    public List<Feedback>? Feedbacks { get; set; }
    public List<SearchHistory>? SearchHistories { get; set; }
    public List<Complaint>? Complaints { get; set; }
    public List<User_Chat>? Chats { get; set; }
    public Address? Address { get; set; }
    public Statistic? Statistic { get; set; }

}
