using Ec.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Complaint
{
    [Key]   
    public int Id { get; set; }
    [Required(ErrorMessage ="Enter text")]
    public string Text { get; set; }
    public ComplaintStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }

    public Guid ClientId { get; set; }
    public User Client { get; set; }
}
