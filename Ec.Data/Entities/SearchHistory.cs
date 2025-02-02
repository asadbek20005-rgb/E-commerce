using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class SearchHistory
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage ="Enter query")]
    public string SearchQuery { get; set; }
    public DateTime CreatedDate  { get; set; }

    public Guid ClientId { get; set; }
    public virtual User Client { get; set; }
}
