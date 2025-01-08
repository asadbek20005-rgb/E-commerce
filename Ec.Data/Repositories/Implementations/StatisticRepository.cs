using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class StatisticRepository(AppDbContext appDbContext) : IRepository<Statistic>
{
    private readonly AppDbContext _contex = appDbContext;
    public async Task AddAsync(Statistic entity)
    {
        await _contex.Statistics.AddAsync(entity);
        await _contex.SaveChangesAsync();
    }

    public async Task DeleteAsync(Statistic entity)
    {
        _contex.Statistics.Remove(entity);
        await _contex.SaveChangesAsync();
    }

    public async Task<List<Statistic>> GetAllAsync()
    {
        var statistics = await _contex.Statistics.AsNoTracking().ToListAsync();
        return statistics;
    }

    public async Task<Statistic> GetByIdAsync(Guid id)
    {
        var statistic = await _contex.Statistics.FindAsync(id);
        return statistic;
    }

    public async Task UpdateAsync(Statistic entity)
    {
        _contex.Statistics.Update(entity);
        await _contex.SaveChangesAsync();
    }
}
