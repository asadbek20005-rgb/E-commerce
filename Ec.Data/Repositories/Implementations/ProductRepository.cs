using Ec.Data.Context;
using Ec.Data.Entities;
using Ec.Data.Enums;
using Ec.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
    public async Task<List<Product>> GetProductsByCategory(Category category)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(product => product.Category == category).ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetProductsByPrice(decimal price)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(product => product.Price == price).ToListAsync();
        return products;
    }   


    public async Task<List<Product>> GetProductsByPriceRange(decimal firstPrice, decimal lastPrice)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(product => product.Price >= firstPrice && product.Price <= lastPrice).ToListAsync();
        return products;
    }


    public async Task<Product> GetProductById(Guid sellerId, Guid productId)
    {
        var product = await _context.Products
            .SingleOrDefaultAsync(x => x.SellerId == sellerId && x.Id == productId);
        return product;
    }

    public async Task<Product> GetProductById(Guid productId)
    {
        var product = await _context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(product => product.Id == productId);
        return product;
    }

    public async Task<List<Product>> GetProductsByCategory(Guid sellerId, Category category)
    {
        var products = await _context.Products.AsNoTracking().Where(x => x.SellerId == sellerId && x.Category == category).ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetProductsByDate(Guid sellerId, string date)
    {
        if (DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var dateUtc))
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(x => x.SellerId == sellerId && x.CreatedDate == dateUtc)
                .ToListAsync();
            return products;
        }
        return null;
    }


    public async Task<List<Product>> GetProductsByPrice(Guid sellerId, decimal price)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(x => x.SellerId == sellerId && x.Price == price)
            .ToListAsync();
        return products;
    }

    public async Task<List<Product>> GetProductsByPriceRange(Guid sellerId, decimal? startedPrice = null, decimal? endedPrice = null)
    {
        var products = await _context.Products
             .AsNoTracking()
             .Where(x => x.SellerId == sellerId && x.Price >= startedPrice && x.Price <= endedPrice)
             .ToListAsync();
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
