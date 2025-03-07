﻿using Ec.Common.Constants;
using Ec.Common.DtoModels;
using Ec.Common.Models.Admin;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Exceptions;
using Ec.Service.Extentions;
using Ec.Service.Helpers;
using Ec.Service.In_memory_Storage;
using Ec.Service.Jwt;

namespace Ec.Service.Api.Admin;

public class AdminService(IUserRepository userRepository,
    RedisService redisService,
    IProductRepository productRepository,
    JwtService jwtService)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly RedisService _redisService = redisService;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly JwtService _jwtService = jwtService;
    public async Task<string> Login(AdminLoginModel model)
    {
        try
        {
            var admin = await IsHaveAdmin(model) ?? throw new Exception("Admin not found");
            Helper.VerfyPassword(admin, model.Password);
            string token = _jwtService.GenerateToken(admin);
            return token;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<UserDto>> GetUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = users.ParseToDtos();
            await _redisService.Set(Constants.UserDtos, userDtos);
            return userDtos;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<UserDto> GetUserById(Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null)
            throw new UserNotFoundException();

        return user.ParseToDto();
    }
    public async Task<ProductDto> GetProductById(Guid productId)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product == null)
            throw new ProductNotFoundException();
        return product.ParseToDto();
    }
    public async Task<List<ProductDto>> GetProductsAsync()
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
    public async Task<bool> BlogUser(Guid userId)
    {
        try
        {

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                throw new UserNotFoundException();
            user.IsBlocked = true;
            await _userRepository.UpdateAsync(user);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> UnblogUser(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                throw new UserNotFoundException();
            user.IsBlocked = false;
            await _userRepository.UpdateAsync(user);
            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> DeleteUser(Guid userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user is null)
            throw new UserNotFoundException();
        await _userRepository.DeleteAsync(user);
        return true;
    }
    public async Task<bool> DeleteProduct(Guid productId)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product is null)
            throw new ProductNotFoundException();
        await _productRepository.DeleteAsync(product);
        return true;
    }

    private async Task<User> IsHaveAdmin(AdminLoginModel model)
    {
        try
        {

            var admin = await _userRepository.GetUserByUsername(model.Username);
            return admin;
        }
        catch (NoSuchAccountExist ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
