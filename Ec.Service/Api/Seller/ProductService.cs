using Ec.Common.Constants;
using Ec.Common.DtoModels;
using Ec.Common.Models.Product;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Ec.Service.Helpers;
using Ec.Service.In_memory_Storage;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ec.Service.Api.Seller;

public class ProductService(IProductRepository productRepository, RedisService redisService)
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly RedisService _redisService = redisService;

    public async Task<ProductDto> Create(Guid sellerId, CreateProductModel model)
    {
        var user = await CheckSellerId(sellerId);
        CheckSellerRole(user);
        await IsUniqueProductName(model.Name);
        CheckPrice(model.Price);
        var newProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            Category = model.Category,
            Status = model.Status,
            SellerId = sellerId,
        };
        var productDto = newProduct.ParseToDto();
        Helper.ProductDtos.Add(productDto);
        await _redisService.Set(Constants.ProductDtos, Helper.ProductDtos);
        await _productRepository.AddAsync(newProduct);
        return productDto;
    }
    public async Task<List<ProductDto>> GetProducts(Guid sellerId)
    {
        var seller = await CheckSellerId(sellerId);
        CheckSellerRole(seller);
        var products = await _productRepository.GetProductsBySellerId(sellerId);
        return products.ParseToDtos();
    }
    public async Task<ProductDto> GetProduct(Guid sellerId, Guid productId)
    {
        var seller = await CheckSellerId(sellerId);
        CheckSellerRole(seller);
        var product = await _productRepository.GetProductById(sellerId, productId);
        if (product == null)
            throw new Exception("Product not found");
        return product.ParseToDto();
    }



    private void CheckPrice(decimal price)
    {
        if (price < 0 || price == 0)
            throw new Exception("Price is null or 0");
    }
    private void CheckSellerRole(UserDto user)
    {
        var isSeller = user.Role == Constants.SellerRole;
        if (!isSeller)
            throw new Exception("User must be a seller");
    }
    private async Task<UserDto> CheckSellerId(Guid sellerId)
    {
        RedisValue redisValue = await _redisService.Get(Constants.UserDtos);
        if (redisValue.HasValue)
        {
            var userDtos = JsonConvert.DeserializeObject<List<UserDto>>(redisValue);
            var seller = userDtos.SingleOrDefault(x => x.Id == sellerId);
            if (seller is null)
                throw new Exception("Seller must be available");
            return seller;
        }
        return null;
    }
    private async Task IsUniqueProductName(string name)
    {

        var products = await _productRepository.GetAllAsync();
        if (products is null)
            return;
        var IsUniqueName = products.Any(products => products.Name == name);
        if (IsUniqueName)
            throw new Exception("There is a product created with this name");
    }

}
