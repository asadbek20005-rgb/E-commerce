using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class User_ChatRepository(AppDbContext appDbContext) : IRepository<User_Chat>
{
    private readonly AppDbContext _context = appDbContext;
    public async Task AddAsync(User_Chat entity)
    {
        await _context.User_Chats.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User_Chat entity)
    {
        _context.User_Chats.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User_Chat>> GetAllAsync()
    {
        var user_chats = await _context.User_Chats.AsNoTracking().ToListAsync();
        return user_chats;
    }
    public async Task<User_Chat> GetByIdAsync(Guid id)
    {
        var user_chat = await _context.User_Chats.FindAsync(id);
        return user_chat;
    }

    public async Task UpdateAsync(User_Chat entity)
    {
        _context.User_Chats.Update(entity);
        await _context.SaveChangesAsync();
    }

}
