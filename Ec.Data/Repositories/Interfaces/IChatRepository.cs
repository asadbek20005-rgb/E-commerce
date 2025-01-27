using Ec.Data.Entities;
namespace Ec.Data.Repositories.Interfaces;

public interface IChatRepository : IRepository<Chat>
{
    Task<(bool, Chat)> CheckChatExist(Guid fromUserId, Guid toUserId);
    Task<List<Chat>> GetUserChats(Guid userId);
    Task<Chat> GetChatById(Guid userId,Guid chatId);
}