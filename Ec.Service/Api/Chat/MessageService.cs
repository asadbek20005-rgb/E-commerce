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




    public async Task<MessageDto> UpdateTextMessage(Guid userId, Guid chatId, int messageId, string newTextMess)
    {
        var message = await GetMessageAsync(userId, chatId, messageId);
        message.Text = newTextMess;
        await _messageRepository.UpdateAsync(message);
        return message.ParseToDto();
    }

    private async Task<Message> GetMessageAsync(Guid userId, Guid chatId, int messageId)
    {
        var user = await GetUserAsync(userId);
        var chat = await GetChatAsync(user.Id, chatId);
        var message = await _messageRepository.GetMessageById(user.Id, chat.Id, messageId);
        CheckMessageExist(message);
        return message;
    }


    private void CheckMessageExist(Message message)
    {
        if (message is null) throw new Exception("Message Not Found");
    }

    private async Task<Data.Entities.Chat> GetChatAsync(Guid userId, Guid chatId)
    {
        var chat = await _chatRepository.GetChatById(userId, chatId);
        CheckChatExist(chat);
        return chat;
    }
    private void CheckChatExist(Data.Entities.Chat chat)
    {
        if (chat is null)
        {
            throw new Exception("Chat Not Found");
        }
    }

    private async Task<User> GetUserAsync(Guid userId)
    {
        User user = await _userRepository.GetUserById(userId);
        CheckUserExist(user);
        return user;
    }
    private void CheckUserExist(User user)
    {
        if (user is null)
            throw new Exception("User Not Found");
    }

    public async Task<MessageDto> GetMessageById(Guid userId, Guid chatId, int messageId)
    {
        var user = await GetUserAsync(userId);
        var chat = await GetChatAsync(user.Id, chatId);
        var message = await GetMessageAsync(user.Id, chat.Id, messageId);
        return message.ParseToDto();
    }



    public async Task<bool> DeleteTextMess(Guid userId, Guid chatId, int messageId)
    {
        var user = await GetUserAsync(userId);
        var chat = await GetChatAsync(user.Id, chatId);
        var message = await GetMessageAsync(userId, chat.Id, messageId);
        await _messageRepository.DeleteAsync(message);
        return true;
    }
}
