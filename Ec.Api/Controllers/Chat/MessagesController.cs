using Ec.Common.Models.Message;
using Ec.Service.Api.Chat;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Chat;

[Route("api/Users/{userId:guid}/Chats/{chatId:guid}/[controller]")]
[ApiController]
public class MessagesController(MessageService messageService) : ControllerBase
{
    private readonly MessageService _messageService = messageService;

    [HttpPost("text")]
    public async Task<IActionResult> SendText(Guid userId, Guid chatId, string text)
    {
        var messageDto = await _messageService.SendTextMessage(userId, chatId, text);
        return Ok(messageDto);
    }

    [HttpPost("file")]
    public async Task<IActionResult> SendFile(Guid userId, Guid chatId, [FromForm] FileModel file)
    {
        var messageDto = await _messageService.SendFileMessage(userId, chatId, file);
        return Ok(messageDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetChatMessages(Guid userId, Guid chatId)
    {
        var messages = await _messageService.GetChatMessages(userId, chatId);
        return Ok(messages);
    }
}
