using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;

namespace Ec.Data.Repositories.Implementations;

public class ProductContentRepostory(AppDbContext appDbContext) : IProductContentRepository
{
    private readonly AppDbContext _context = appDbContext;
    public async Task Add(ProductContent productContent)
    {
        await _context.AddAsync(productContent);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(ProductContent productContent)
    {
        _context.Remove(productContent);
        await _context.SaveChangesAsync();
    }

    public async Task Update(ProductContent productContent)
    {
        _context.Update(productContent);
        await _context.SaveChangesAsync();
    }
}
