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
    IUserRepository userRepository)
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly RedisService _redisService = redisService;
    private readonly MinioService _minioService = minioService;
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<ProductDto> Create(Guid sellerId, CreateProductModel model, Category category, ProductStatus status)
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
            SellerId = sellerId,
        };

        newProduct.Category = category; newProduct.Status = status;
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
    public async Task<string> UploadFile(Guid sellerId, Guid productId, IFormFile file)
    {
        var seller = await CheckSellerId(sellerId);
        CheckSellerRole(seller);
        var product = await CheckProductId(sellerId, productId);
        if (file == null) throw new ArgumentNullException(nameof(file));

        var (fileName, contentType, size, data) = await SaveFileDetails(file);
        await _minioService.UploadFileAsync(fileName, data, size, contentType);
        product.VideoUrl = fileName;
        await _productRepository.UpdateAsync(product);
        return fileName;
    }
    public async Task<Stream> GetFileAsync(Guid sellerId,Guid productId,string fileName)
    {
        var seller = await CheckSellerId(sellerId);
        CheckSellerRole(seller);
        _ = await CheckProductId(sellerId, productId);
        var ms = await _minioService.GetFileAsync(fileName);
        return ms;
    }
    public async Task<List<ProductDto>> GetProductsByPrice(Guid sellerId, decimal price)
    {
        var seller = await CheckSellerId(sellerId);
        CheckSellerRole(seller);
        var products = await _productRepository.GetProductsByPrice(seller.Id, price);
        return products.ParseToDtos();
    }
    public async Task<List<ProductDto>> GetProductsByPriceRange(Guid sellerId, decimal? startedPrice, decimal? endedPrice)
    {
        var seller = await CheckSellerId(sellerId);
        CheckSellerRole(seller);
        var products = await _productRepository.GetProductsByPriceRange(seller.Id, startedPrice, endedPrice);
        return products.ParseToDtos();
    }
    public async Task<List<ProductDto>> GetProductsByDate(Guid sellerId, string date)
    {
        var seller = await CheckSellerId(sellerId);
        CheckSellerRole(seller);
        var products = await _productRepository.GetProductsByDate(seller.Id, date);
        return products.ParseToDtos();
    }
    public async Task<bool> UpdateProductByPrice(Guid sellerId, Guid productId, decimal price)
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
    public async Task<bool> UpdateProductByDescription(Guid sellerId, Guid productId, string descrption)
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
    public async Task<bool> UpdateProductByStatus(Guid sellerId, Guid productId, ProductStatus status)
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
    public async Task<bool> UpdateProductByName(Guid sellerId, Guid productId, string name)
    {
        var seller = await CheckSellerId(sellerId);
        CheckSellerRole(seller);
        var product = await CheckProductId(seller.Id, productId);
        if (string.IsNullOrEmpty(name)) throw new Exception("Name is null");
        product.Name = name;
        await _productRepository.UpdateAsync(product);
        return true;
    }
    public async Task<bool> UpdateProduct(Guid sellerId, Guid productId, UpdateProductModel model)
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
    public async Task<bool> DeleteProduct(Guid sellerId, Guid productId)
    {
        var seller = await CheckSellerId(sellerId);
        CheckSellerRole(seller);
        var product = await CheckProductId(seller.Id, productId);
        await _productRepository.DeleteAsync(product);
        return true;
    }





    private async Task<(string FileName, string ContentType, long Size, MemoryStream Data)> SaveFileDetails(IFormFile file)
    {
        var fileName = Guid.NewGuid().ToString();
        string contentType = file.ContentType;
        long size = file.Length;

        var data = new MemoryStream();
        await file.CopyToAsync(data);

        return (fileName, contentType, size, data);
    }
    private async Task<Product> CheckProductId(Guid sellerId, Guid productId)
    {
        var product = await _productRepository.GetProductById(sellerId, productId);
        if (product == null) throw new Exception("Product Not Found");
        return product;
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
