using Ec.Data.Entities;

namespace Ec.Data.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetProductsByPrice(decimal price, decimal startedPrice, decimal endedPrice);
    Task<List<Product>> GetProductsByCategory(string category);
    Task<List<Product>> GetProductsBySellerId(Guid sellerId);
    Task<Product> GetProductById(Guid sellerId,Guid productId);

}