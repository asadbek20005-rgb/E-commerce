using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class ChatRepository(AppDbContext appDbContext) : IChatRepository
{
    private readonly AppDbContext _context = appDbContext;
    public async Task AddAsync(Chat entity)
    {
        await _context.Chats.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<(bool, Chat)> CheckChatExist(Guid fromUserId, Guid toUserId)
    {
        var userChat = await _context.User_Chats
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == fromUserId && x.ToUserId == toUserId);

        if (userChat == null)
            return (false, null);

        var chat = await GetChatByIdAsync(userChat.UserId, userChat.ChatId);
        return (true, chat);
    }

    public async Task DeleteAsync(Chat entity)
    {
        _context.Chats.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Chat>> GetAllAsync()
    {
        var chats = await _context.Chats.AsNoTracking().ToListAsync();
        return chats;
    }

    public async Task<Chat> GetChatById(Guid userId, Guid chatId)
    {
        var chat = await _context.Chats
            .Where(c => c.Id == chatId && c.Users.Any(uc => uc.UserId == userId))
            .SingleOrDefaultAsync();

        if (chat == null)
            throw new Exception("Chat not found");

        return chat;
    }



    public async Task<Chat> GetChatByIdAsync(Guid userId, Guid chatId)
    {
        var userChat = await _context.User_Chats
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.ChatId == chatId);

        var chat = await _context.Chats
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == userChat.ChatId);

        return chat;
    }

    public async Task<List<Chat>> GetUserChats(Guid userId)
    {
        var userChats = await _context.User_Chats
           .AsNoTracking()
           .Where(uch => uch.UserId == userId)
           .ToListAsync();
        var chats = new List<Chat>();
        if (userChats.Count == 0 || userChats is null)
            return chats;

        foreach (var chat in userChats)
        {
            var sortedChat = await _context.Chats
                .AsNoTracking()
                .SingleAsync(x => x.Id == chat.ChatId);
            chats.Add(sortedChat);
        }
        return chats;
    }

    public async Task UpdateAsync(Chat entity)
    {
        _context.Chats.Update(entity);
        await _context.SaveChangesAsync();
    }
}
