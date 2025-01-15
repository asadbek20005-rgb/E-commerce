using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ec.Data.Repositories.Implementations;

public class ProductRepository(AppDbContext appDbContext) : IProductRepository
{
    private readonly AppDbContext _context = appDbContext;
    public async Task AddAsync(Product entity)
    {
        await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetAllAsync()
    {
        var products = await _context.Products.AsNoTracking().ToListAsync();
        return products;
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        return product;
    }

    public async Task<Product> GetProductById(Guid sellerId,Guid productId)
    {
        var product = await _context.Products
            .SingleOrDefaultAsync(x => x.SellerId == sellerId && x.Id == productId);
        return product;
    }

    public async Task<List<Product>> GetProductsByCategory(string category)
    {
        var products = await _context.Products.AsNoTracking().Where(x => x.Category == category).ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetProductsByPrice(decimal price, decimal startedPrice, decimal endedPrice)
    {
        var products = new List<Product>();
        if (price > 0)
        {
            products = await _context.Products.AsNoTracking().Where(x => x.Price == price).ToListAsync();
        }
        else
        {
            products = await _context.Products.AsNoTracking().Where(x => x.Price <= startedPrice && x.Price >= endedPrice).ToListAsync();
            return products;
        }
        return products;
    }

    public async Task<List<Product>> GetProductsBySellerId(Guid sellerId)
    {
        var products = await _context.Products
            .Where(x => x.SellerId == sellerId).ToListAsync();
        return products;
    }

    public async Task UpdateAsync(Product entity)
    {
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
    }
}
