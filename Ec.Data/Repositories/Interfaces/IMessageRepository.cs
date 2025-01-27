using Ec.Data.Entities;

namespace Ec.Data.Repositories.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    Task<List<Message>> GetChatMessages(Guid userId, Guid chatId);
}
