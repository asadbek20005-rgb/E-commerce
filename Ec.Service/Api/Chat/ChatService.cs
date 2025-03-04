using Ec.Common.DtoModels;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Exceptions;
using Ec.Service.Extentions;
using Ec.Service.Helpers;

namespace Ec.Service.Api.Chat;

public class ChatService(IChatRepository chatRepository,
    IUserRepository userRepository,
    IUser_ChatRepository user_ChatRepository)
{
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUser_ChatRepository _user_ChatRepository = user_ChatRepository;

    public async Task<ChatDto> AddOrEnterChat(Guid fromUserId, Guid toUserId)
    {
        var (isExist, chat) = await _chatRepository.CheckChatExist(fromUserId, toUserId);
        if (isExist) return chat.ParseToDto();

        var fromUser = await _userRepository.GetUserById(fromUserId);
        var toUser = await _userRepository.GetUserById(toUserId);

        chat = await CreateChat(fromUser.FullName, toUser.FullName);
        await AddUserChats(chat.Id, fromUserId, toUserId);

        return chat.ParseToDto();
    }
    private async Task<Data.Entities.Chat> CreateChat(string fromUserName, string toUserName)
    {
        var chat = new Data.Entities.Chat { Names = new List<string> { fromUserName, toUserName } };
        await _chatRepository.AddAsync(chat);
        return chat;
    }
    private async Task AddUserChats(Guid chatId, Guid fromUserId, Guid toUserId)
    {
        await _user_ChatRepository.AddAsync(new User_Chat { UserId = fromUserId, ToUserId = toUserId, ChatId = chatId });
        await _user_ChatRepository.AddAsync(new User_Chat { UserId = toUserId, ToUserId = fromUserId, ChatId = chatId });
    }
    public async Task<List<ChatDto>> GetChats(Guid userId)
    {
        try
        {
            var user = await CheckUserExistById(userId);
            UserExtension.CheckUserRole(user.Role);
            var chats = await _chatRepository.GetUserChats(userId);
            return chats.ParseToDtos();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private async Task<User> CheckUserExistById(Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
            throw new UserNotFoundException();
        return user;
    }

    public async Task<ChatDto> UpdateChatName(Guid userId, Guid chatId, string newChatName)
    {
        CheckName(newChatName);
        var (chat, user) = await GetChatAndUser(userId, chatId);
        UpdateName(chat, user, newChatName);
        await _chatRepository.UpdateAsync(chat);
        return chat.ParseToDto();
    }

    private void UpdateName(Ec.Data.Entities.Chat chat, User user, string name)
    {
        var names = chat.Names;

        for (int i = 0; names.Count > i; i++)
        {
            if (user.FullName != names[i])
            {
                names[i] = name;
            }
        }
    }

    private void CheckName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new Exception("Null chat-name can not be updated");
    }

    private async Task<Tuple<Data.Entities.Chat, User>> GetChatAndUser(Guid userId, Guid chatId)
    {
        var user = await CheckUserExistById(userId);
        Helper.CheckUserExist(user);
        var chat = await _chatRepository.GetChatById(user.Id, chatId);
        Helper.CheckChatExist(chat);
        return new(chat, user);
    }


    public async Task<bool> DeletChat(Guid userId, Guid chatId)
    {
        var (chat, user) = await GetChatAndUser(userId, chatId);
        await _chatRepository.DeleteAsync(chat);
        return true;
    }
}