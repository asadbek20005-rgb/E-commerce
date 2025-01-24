using Ec.Data.Entities;
using Ec.Data.Enums;

namespace Ec.Data.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetProductsByPrice(Guid userId, decimal price);
    Task<List<Product>> GetProductsByPriceRange(Guid userId,decimal? firstPrice = null, decimal? lastPrice = null);
    Task<List<Product>> GetProductsByCategory(Guid userId,Category category);
    Task<List<Product>> GetProductsBySellerId(Guid userId);
    Task<Product> GetProductById(Guid userId,Guid productId);
    Task<List<Product>> GetProductsByDate(Guid userId, string date);
    Task<Product> GetProductById(Guid productId);
    Task<List<Product>> GetProductsByCategory(Category category);
    Task<List<Product>> GetProductsByPrice(decimal price);
    Task<List<Product>> GetProductsByPriceRange(decimal firstPrice, decimal lastPrice);



}