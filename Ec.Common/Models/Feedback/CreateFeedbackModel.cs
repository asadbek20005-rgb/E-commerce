using Ec.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Ec.Common.Models.Feedback;

public class CreateFeedbackModel
{
    [Required(ErrorMessage = "Enter comment")]
    public string Comment { get; set; }
}
