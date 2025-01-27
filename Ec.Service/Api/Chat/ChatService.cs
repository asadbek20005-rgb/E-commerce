using Ec.Common.DtoModels;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;

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
        try
        {
            var (isExist, chat) = await _chatRepository.CheckChatExist(fromUserId, toUserId);
            if (isExist)
                return chat.ParseToDto();
            var fromUser = await _userRepository.GetUserById(fromUserId);
            var toUser = await _userRepository.GetUserById(toUserId);

            var chatNames = new List<string>
            {
            $"{fromUser.FullName}",
            $"{toUser.FullName}"
            };

            chat = new Data.Entities.Chat
            {
                Names = chatNames
            };
            await _chatRepository.AddAsync(chat);

            var fromUserChat = new User_Chat
            {
                UserId = fromUserId,
                ToUserId = toUserId,
                ChatId = chat.Id
            };
            await _user_ChatRepository.AddAsync(fromUserChat);

            var toUserChat = new User_Chat
            {
                UserId = toUserId,
                ToUserId = fromUserId,
                ChatId = chat.Id
            };
            await _user_ChatRepository.AddAsync(toUserChat);

            return chat.ParseToDto();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

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
}