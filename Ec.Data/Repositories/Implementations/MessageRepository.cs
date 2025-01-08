using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class MessageRepository(AppDbContext appDbContext) : IRepository<Message>
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

    public async Task UpdateAsync(Message entity)
    {
        _context.Messages.Update(entity);
        await _context.SaveChangesAsync();
    }
}
