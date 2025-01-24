using Ec.Common.DtoModels;
using Ec.Data.Enums;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Ec.Service.Minio;

namespace Ec.Service.Api.Client;

public class ProductService(IProductRepository productRepository, IUserRepository userRepository, MinioService minioService)
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly MinioService _minioService = minioService;
    public async Task<List<ProductDto>> GetProducts()
    {
        var products = await _productRepository.GetAllAsync();
        return products.ParseToDtos();
    }
    public async Task<ProductDto> GetProductById(Guid productId)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product == null)
            throw new Exception("Product Not Found");
        return product.ParseToDto();
    }
    public async Task<List<ProductDto>> GetProductsByCategory(Category category)
    {
        var products = await _productRepository.GetProductsByCategory(category);
        return products.ParseToDtos();
    }
    public async Task<List<ProductDto>> GetProductsByPrice(decimal price)
    {
        var products = await _productRepository.GetProductsByPrice(price);
        return products.ParseToDtos();
    }

    public async Task<List<ProductDto>> GetProductsByPriceRange(decimal startPrice, decimal endPrice)
    {

        var products = await _productRepository.GetProductsByPriceRange(startPrice, endPrice);
        return products.ParseToDtos();
    }
    public async Task<Stream> GetVideo(Guid productId, string fileName)
    {

        await CheckProductExistAsync(productId);
        var ms = await _minioService.GetFileAsync(fileName);
        return ms;
    }



    private async Task CheckProductExistAsync(Guid productId)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product is null)
            throw new Exception("Product Not Found");
    }

}