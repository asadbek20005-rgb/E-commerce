using Ec.Service.Api.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Chat;

[Route("api/[controller]")]
[ApiController]
public class ChatsController(ChatService chatService) : ControllerBase
{
    private readonly ChatService _chatService = chatService;
    [HttpPost]
    public async Task<IActionResult> EnterOrAddChat(Guid fromUserId, Guid toUserId)
    {
        var chat = await _chatService.AddOrEnterChat(fromUserId, toUserId);
        return Ok(chat);
    }
}
