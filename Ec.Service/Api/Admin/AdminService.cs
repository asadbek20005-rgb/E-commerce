using Ec.Common.Constants;
using Ec.Common.DtoModels;
using Ec.Common.Models.Admin;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Ec.Service.Helpers;
using Ec.Service.In_memory_Storage;

namespace Ec.Service.Api.Admin;

public class AdminService(IUserRepository userRepository, RedisService redisService)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly RedisService _redisService = redisService;

    public async Task<string> Login(AdminLoginModel model)
    {
        var admin = await IsHaveAdmin(model);
        Helper.VerfyPassword(admin, model.Password);
        return "Successfull";
    }
    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _redisService.GetUsersAsync(Constants.GetUsersAsyncCacheKey);
        var sellersAndClients = new List<User>();
        if (users is not null)
        {
            sellersAndClients = users.Where(x => x.Role == Constants.ClientRole || x.Role == Constants.SellerRole).ToList();
            return sellersAndClients.ParseToDtos();
        }
        users = await _userRepository.GetAllAsync();
        sellersAndClients = users.Where(x => x.Role == Constants.ClientRole || x.Role == Constants.SellerRole).ToList();
        await _redisService.SetUsersAsync(Constants.GetUsersAsyncCacheKey, users);
        return users.ParseToDtos();
    }
    private async Task<User> IsHaveAdmin(AdminLoginModel model)
    {

        var users = await _redisService.GetUsersAsync(Constants.GetUsersAsyncCacheKey);
        if (users is not null)
        {
            var adminCache = users.FirstOrDefault(x => x.Username == model.Username);
            if (adminCache is not null) return adminCache;
        }

        users = await _userRepository.GetAllAsync();
        var admin = users.SingleOrDefault(x => x.Username == model.Username);
        if (admin is null)
            throw new Exception("no such account exists");
        await _redisService.SetUsersAsync(Constants.GetUsersAsyncCacheKey, users);
        return admin;
    }
}
