using Ec.Common.Models.Feedback;
using Ec.Data.Enums;
using Ec.Service.Api.Client;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Client;

[Route("api/Clients/{clientId:guid}/[controller]")]
[ApiController]
public class FeedbacksController(FeedbackService feedbackService) : ControllerBase
{
    private readonly FeedbackService _feedbackService = feedbackService;

    [HttpPost]
    public async Task<IActionResult> Create(Guid clientId,[FromQuery] Guid sellerId, [FromQuery] Guid productId, CreateFeedbackModel model, Rank rank)
    {
        await _feedbackService.Create(clientId,sellerId,productId,model, rank);
        return Ok(model);
    }
}
