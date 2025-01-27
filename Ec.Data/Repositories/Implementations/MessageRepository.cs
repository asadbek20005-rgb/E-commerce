using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class MessageRepository(AppDbContext appDbContext) : IMessageRepository
{
    private readonly AppDbContext _context = appDbContext;
    public async Task AddAsync(Message entity)
    {
        await _context.Messages.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Message entity)
    {
        _context.Messages.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Message>> GetAllAsync()
    {
        var messages = await _context.Messages.AsNoTracking().ToListAsync();
        return messages;
    }

    public async Task<Message> GetByIdAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        return message;
    }

    public async Task<List<Message>> GetChatMessages(Guid userId, Guid chatId)
    {
        var userChat = await _context.User_Chats
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.ChatId == chatId)
            .FirstOrDefaultAsync();
        if (userChat == null)
            throw new Exception("User Chat Not Found");
        var messages = await _context.Messages
            .AsNoTracking()
            .Where(x => x.ChatId == userChat.ChatId)
            .ToListAsync();
        return messages;

    }

    public async Task UpdateAsync(Message entity)
    {
        _context.Messages.Update(entity);
        await _context.SaveChangesAsync();
    }
}
