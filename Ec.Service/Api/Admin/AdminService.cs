using Ec.Common.Constants;
using Ec.Common.DtoModels;
using Ec.Common.Models.Admin;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Ec.Service.Helpers;
using Ec.Service.In_memory_Storage;

namespace Ec.Service.Api.Admin;

public class AdminService(IUserRepository userRepository, RedisService redisService, IProductRepository productRepository)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly RedisService _redisService = redisService;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<string> Login(AdminLoginModel model)
    {
        try
        {
            var admin = await IsHaveAdmin(model);
            Helper.VerfyPassword(admin, model.Password);
            return "Successfull";

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
                throw new Exception("User not found");
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
                throw new Exception("User not found");
            user.IsBlocked = false;
            await _userRepository.UpdateAsync(user);
            return true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    private async Task<User> IsHaveAdmin(AdminLoginModel model)
    {
        try
        {

            var admin = await _userRepository.GetUserByUsername(model.Username);
            if (admin is null)
                throw new Exception("no such account exists");
            return admin;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
