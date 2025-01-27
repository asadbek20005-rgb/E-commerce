using Ec.Service.Api.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Chat;

[Route("api/Users/{userId:guid}/Chats/{chatId:guid}/[controller]")]
[ApiController]
public class MessagesController(MessageService messageService) : ControllerBase
{
    private readonly MessageService _messageService = messageService;

    [HttpPost("text")]
    public async Task<IActionResult> Send(Guid userId, Guid chatId, string text)
    {
        var messageDto = await _messageService.SendMessage(userId, chatId, text);
        return Ok(messageDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetChatMessages(Guid userId, Guid chatId)
    {
        var messages = await _messageService.GetChatMessages(userId, chatId);
        return Ok(messages);
    }
}
