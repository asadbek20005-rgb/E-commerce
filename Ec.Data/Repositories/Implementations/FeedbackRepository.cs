using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class FeedbackRepository(AppDbContext appDbContext) : IFeedbackRepository
{
    private readonly AppDbContext _context = appDbContext;
    public async Task AddAsync(Feedback entity)
    {
        await _context.Feedbacks.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Feedback entity)
    {
        _context.Feedbacks.Remove(entity);
        await _context.SaveChangesAsync(true);
    }

    public async Task<List<Feedback>> GetAllAsync()
    {
        var feedbacks = await _context.Feedbacks.AsNoTracking().ToListAsync();
        return feedbacks;
    }

    public async Task<Feedback> GetByIdAsync(Guid id)
    {
        var feedback = await _context.Feedbacks.FindAsync(id);
        return feedback;
    }

    public async Task UpdateAsync(Feedback entity)
    {
        _context.Feedbacks.Update(entity);
        await _context.SaveChangesAsync();
    }
}
