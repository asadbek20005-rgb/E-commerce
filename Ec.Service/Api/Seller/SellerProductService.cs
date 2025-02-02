using Ec.Common.Constants;
using Ec.Common.DtoModels;
using Ec.Common.Models.Product;
using Ec.Data.Entities;
using Ec.Data.Enums;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Ec.Service.Helpers;
using Ec.Service.In_memory_Storage;
using Ec.Service.Minio;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ec.Service.Api.Seller;

public class SellerProductService(IProductRepository productRepository,
    RedisService redisService,
    MinioService minioService,
    IUserRepository userRepository,
    IProductContentRepository productContentRepository)
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly RedisService _redisService = redisService;
    private readonly MinioService _minioService = minioService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IProductContentRepository _productContentRepository = productContentRepository;



    public async Task<ProductDto> Create(Guid sellerId, CreateProductModel model, Category category, ProductStatus status)
    {
        try
        {

            var user = await CheckSellerId(sellerId);
            CheckSellerRole(user);
            await IsUniqueProductName(model.Name);
            Helper.CheckPrice(model.Price);
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                SellerId = sellerId,
            };

            newProduct.Category = category; newProduct.Status = status;
            var productDto = newProduct.ParseToDto();
            Helper.ProductDtos.Add(productDto);
            await _redisService.Set(Constants.ProductDtos, Helper.ProductDtos);
            await _productRepository.AddAsync(newProduct);
            return productDto;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<ProductDto>> GetProducts(Guid sellerId)
    {

        try
        {
            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var products = await _productRepository.GetProductsBySellerId(sellerId);
            return products.ParseToDtos();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<ProductDto> GetProduct(Guid sellerId, Guid productId)
    {
        try
        {
            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var product = await _productRepository.GetProductById(sellerId, productId);
            if (product == null)
                throw new Exception("Product not found");
            return product.ParseToDto();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<string> UploadFile(Guid sellerId, Guid productId, IFormFile file, string caption)
    {
        try
        {

            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var product = await CheckProductId(sellerId, productId);
            if (file == null) throw new ArgumentNullException(nameof(file));

            var (fileName, contentType, size, data) = await Helper.SaveFileDetails(file);
            await _minioService.UploadFileAsync(fileName, data, size, contentType);
            var content = new ProductContent
            {
                FileUrl = fileName,
                FileType = contentType,
                Caption = caption,
            };
        
            await _productContentRepository.Add(content);
            return fileName;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public async Task<Stream> GetFileAsync(Guid sellerId, Guid productId, string fileName)
    {
        try
        {
            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            _ = await CheckProductId(sellerId, productId);
            var ms = await _minioService.GetFileAsync(fileName);
            return ms;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<ProductDto>> GetProductsByPrice(Guid sellerId, decimal price)
    {

        try
        {
            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var products = await _productRepository.GetProductsByPrice(seller.Id, price);
            return products.ParseToDtos();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<ProductDto>> GetProductsByPriceRange(Guid sellerId, decimal? startedPrice, decimal? endedPrice)
    {
        try
        {

            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var products = await _productRepository.GetProductsByPriceRange(seller.Id, startedPrice, endedPrice);
            return products.ParseToDtos();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<ProductDto>> GetProductsByDate(Guid sellerId, string date)
    {
        try
        {
            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var products = await _productRepository.GetProductsByDate(seller.Id, date);
            return products.ParseToDtos();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> UpdateProductByPrice(Guid sellerId, Guid productId, decimal price)
    {

        try
        {
            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var product = await CheckProductId(seller.Id, productId);
            if (price <= 0)
                throw new Exception("Enter valid price");
            product.Price = price;
            await _productRepository.UpdateAsync(product);
            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> UpdateProductByDescription(Guid sellerId, Guid productId, string descrption)
    {
        try
        {

            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var product = await CheckProductId(seller.Id, productId);
            if (string.IsNullOrEmpty(descrption))
                throw new Exception("Descripton is null");
            product.Description = descrption;
            await _productRepository.UpdateAsync(product);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public async Task<bool> UpdateProductByStatus(Guid sellerId, Guid productId, ProductStatus status)
    {
        try
        {
            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var product = await CheckProductId(seller.Id, productId);
            if (status <= 0)
                throw new Exception("Enter valid status");
            product.Status = status;
            await _productRepository.UpdateAsync(product);
            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public async Task<bool> UpdateProductByName(Guid sellerId, Guid productId, string name)
    {

        try
        {

            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var product = await CheckProductId(seller.Id, productId);
            if (string.IsNullOrEmpty(name)) throw new Exception("Name is null");
            product.Name = name;
            await _productRepository.UpdateAsync(product);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> UpdateProduct(Guid sellerId, Guid productId, UpdateProductModel model)
    {
        try
        {
            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var product = await CheckProductId(seller.Id, productId);
            if (!string.IsNullOrEmpty(model.Name))
                product.Name = model.Name;
            if (!string.IsNullOrEmpty(model.Description)) product.Description = model.Description;
            if (model.Price > 0)
                product.Price = model.Price;
            if (model.Category >= 0)
                product.Category = model.Category;
            if (model.Status >= 0)
                product.Status = model.Status;
            else return false;

            await _productRepository.UpdateAsync(product);
            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> DeleteProduct(Guid sellerId, Guid productId)
    {
        try
        {
            var seller = await CheckSellerId(sellerId);
            CheckSellerRole(seller);
            var product = await CheckProductId(seller.Id, productId);
            await _productRepository.DeleteAsync(product);
            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }






    private async Task<Product> CheckProductId(Guid sellerId, Guid productId)
    {
        try
        {
            var product = await _productRepository.GetProductById(sellerId, productId);
            if (product == null) throw new Exception("Product Not Found");
            return product;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void CheckSellerRole(UserDto user)
    {
        try
        {
            var isSeller = user.Role == Constants.SellerRole;
            if (!isSeller)
                throw new Exception("User must be a seller");

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
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
        else
        {
            var userEntities = await _userRepository.GetAllAsync();
            var seller = userEntities.SingleOrDefault(x => x.Id == sellerId);
            if (seller is null)
                throw new Exception("Seller not found");
            await _redisService.Set(Constants.UserDtos, userEntities.ParseToDtos());
            return seller.ParseToDto();
        }
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
