using System.ComponentModel.DataAnnotations;

namespace Ec.Common.DtoModels;

public class MessageDto
{
    [Required]
    public string Text { get; set; }
}
