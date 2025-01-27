using Ec.Common.DtoModels;
using Ec.Data.Enums;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Ec.Service.Minio;

namespace Ec.Service.Api.Client;

public class ProductService(IProductRepository productRepository,
    MinioService minioService)
{

    private readonly IProductRepository _productRepository = productRepository;
    private readonly MinioService _minioService = minioService;


    public async Task<List<ProductDto>> GetProducts()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            return products.ParseToDtos();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<ProductDto> GetProductById(Guid productId)
    {
        try
        {

            var product = await _productRepository.GetProductById(productId);
            if (product == null)
                throw new Exception("Product Not Found");
            return product.ParseToDto();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<ProductDto>> GetProductsByCategory(Category category)
    {
        try
        {
            var products = await _productRepository.GetProductsByCategory(category);
            return products.ParseToDtos();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<ProductDto>> GetProductsByPrice(decimal price)
    {
        try
        {

            var products = await _productRepository.GetProductsByPrice(price);
            return products.ParseToDtos();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<ProductDto>> GetProductsByPriceRange(decimal startPrice, decimal endPrice)
    {
        try
        {
            var products = await _productRepository.GetProductsByPriceRange(startPrice, endPrice);
            return products.ParseToDtos();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public async Task<Stream> GetVideo(Guid productId, string fileName)
    {
        try
        {
            await CheckProductExistAsync(productId);
            var ms = await _minioService.GetFileAsync(fileName);
            return ms;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }



    private async Task CheckProductExistAsync(Guid productId)
    {
        try
        {
            var product = await _productRepository.GetProductById(productId);
            if (product is null)
                throw new Exception("Product Not Found");

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}