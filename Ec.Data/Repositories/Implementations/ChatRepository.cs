using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class ChatRepository(AppDbContext appDbContext) : IRepository<Chat>
{
    private readonly AppDbContext _context = appDbContext;
    public async Task AddAsync(Chat entity)
    {
        await _context.Chats.AddAsync(entity);
        await _context.SaveChangesAsync();
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

    public async Task<Chat> GetByIdAsync(Guid id)
    {
        var chat = await _context.Chats.FindAsync(id);
        return chat;
    }

    public async Task UpdateAsync(Chat entity)
    {
        _context.Chats.Update(entity);
        await _context.SaveChangesAsync();
    }
}
