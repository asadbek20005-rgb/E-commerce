using Ec.Common.DtoModels;
using Ec.Common.Models.Message;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Ec.Service.Hubs;
using Ec.Service.Minio;
using Microsoft.AspNetCore.SignalR;

namespace Ec.Service.Api.Chat;

public class MessageService(IMessageRepository messageRepository,
    IUserRepository userRepository,
    IChatRepository chatRepository,
    IHubContext<ChatHub> hubContext,
    MinioService minioService)
{
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IHubContext<ChatHub> _hubContext = hubContext;
    private readonly MinioService _minioService = minioService;

    public async Task<MessageDto> SendTextMessage(Guid userId, Guid chatId, string text)
    {
        try
        {
            var user = await CheckUserExistById(userId);
            UserExtension.CheckUserRole(user.Role);
            var chat = await CheckChatExist(user.Id, chatId);
            var newMessage = new Message
            {
                Text = text,
                ChatId = chat.Id,
                FromUser = user.FullName,
                FromUserId = user.Id,
                SendedAt = DateTime.UtcNow,
            };

            await _hubContext.Clients.All.SendAsync("new_message", newMessage);
            await _messageRepository.AddAsync(newMessage);
            return newMessage.ParseToDto();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public async Task<List<MessageDto>> GetChatMessages(Guid userId, Guid chatId)
    {
        try
        {
            var user = await CheckUserExistById(userId);
            UserExtension.CheckUserRole(user.Role);
            var chat = await CheckChatExist(user.Id, chatId);
            var messages = await _messageRepository.GetChatMessages(user.Id, chat.Id);
            return messages.ParseToDtos();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }




    private async Task<User> CheckUserExistById(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                throw new Exception("User Not Found");
            return user;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private async Task<Data.Entities.Chat> CheckChatExist(Guid userId, Guid chatId)
    {
        try
        {
            var chat = await _chatRepository.GetChatById(userId, chatId);
            if (chat == null) throw new Exception("Chat Not Found");
            return chat;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<MessageDto> SendFileMessage(Guid userId, Guid chatId, FileModel file)
    {
        var user = await CheckUserExistById(userId);
        var chat = await CheckChatExist(user.Id, chatId);
        string objectName = Guid.NewGuid().ToString();
        var data = new MemoryStream();
        long size = file.file.Length;
        string ContentType = file.file.ContentType;
        await file.file.CopyToAsync(data);
        await _minioService.UploadFileAsync(objectName, data, size, ContentType);
        var newContent = new MessageContent
        {
            FileUrl = objectName,
            Caption = file.Caption,
        };

        var newMessage = new Message
        {
            FromUserId = user.Id,
            FromUser = user.FullName,
            ChatId = chat.Id,
            Content = newContent,
        };

        await _messageRepository.AddAsync(newMessage);
        return newMessage.ParseToDto();
    }
}
