﻿using Ec.Common.Constants;
using Ec.Service.Api.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Chat;

[Route("api/Users/{userId:guid}/[controller]")]
[ApiController]
[Authorize(Roles = $"{Constants.ClientRole}, {Constants.SellerRole}")]
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


    [HttpPut("{chatId:guid}/new-chat-name")]
    public async Task<IActionResult> UpdateChatName(Guid userId, Guid chatId, [FromQuery] string chatName)
    {
        var chatDto = await _chatService.UpdateChatName(userId, chatId, chatName);
        return Ok(chatDto);
    }


    [HttpDelete("{chatId:guid}")]
    public async Task<IActionResult> DeleteChat(Guid userId, Guid chatId)
    {
        bool res = await _chatService.DeletChat(userId, chatId);
        return Ok(res);
    }
}
