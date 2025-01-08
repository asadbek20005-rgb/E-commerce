using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class SearchHistoryRepository(AppDbContext appDbContext) : IRepository<SearchHistory>
{
    private readonly AppDbContext _context = appDbContext;
    public async Task AddAsync(SearchHistory entity)
    {
        await _context.SearchHistories.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(SearchHistory entity)
    {
        _context.SearchHistories.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<SearchHistory>> GetAllAsync()
    {
        var searchHistories = await _context.SearchHistories.AsNoTracking().ToListAsync();
        return searchHistories;
    }

    public async Task<SearchHistory> GetByIdAsync(Guid id)
    {
        var searchHistory = await _context.SearchHistories.FindAsync(id);
        return searchHistory;
    }

    public async Task UpdateAsync(SearchHistory entity)
    {
        _context.SearchHistories.Update(entity);
        await _context.SaveChangesAsync();
    }
}
