using Ec.Service.Api.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Chat;

[Route("api/Users/{userId:guid}/[controller]")]
[ApiController]
public class ChatsController(ChatService chatService) : ControllerBase
{
    private readonly ChatService _chatService = chatService;
    [HttpPost]
    public async Task<IActionResult> EnterOrAddChat(Guid userId, Guid toUserId)
    {
        var chat = await _chatService.AddOrEnterChat(userId, toUserId);
        return Ok(chat);
    }
    [HttpGet]
    public async Task<IActionResult> GetUserChats(Guid userId)
    {
        var chats = await _chatService.GetChats(userId);
        return Ok(chats);
    }
}
