using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ec.Common.Models.Message;

public class FileModel
{
    [Required]
    public IFormFile file {  get; set; }
    public string? Caption { get; set; }
}
