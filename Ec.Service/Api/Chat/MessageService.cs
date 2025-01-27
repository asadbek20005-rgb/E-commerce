using Ec.Common.DtoModels;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;

namespace Ec.Service.Api.Chat;

public class MessageService(IMessageRepository messageRepository, IUserRepository userRepository, IChatRepository chatRepository)
{
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IChatRepository _chatRepository = chatRepository;

    public async Task<MessageDto> SendMessage(Guid userId, Guid chatId, string text)
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
            };

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
}
