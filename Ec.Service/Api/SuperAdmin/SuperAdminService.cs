using Ec.Common.Constants;
using Ec.Common.DtoModels;
using Ec.Common.Models.Admin;
using Ec.Data.Entities;
using Ec.Data.Repositories.Interfaces;
using Ec.Service.Extentions;
using Ec.Service.Helpers;
using Ec.Service.In_memory_Storage;

namespace Ec.Service.Api.SuperAdmin;

public class SuperAdminService(IUserRepository userRepository, RedisService redisService)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly RedisService _redisService = redisService;

    public async Task<(string username, string password)> CreateAdmin(AdminCreateModel model)
    {
        await IsUniqueUsername(model.Username);
        var newAdmin = new User
        {
            FullName = model.FullName,
            Username = model.Username,
            Role = Constants.AdminRole
        };
        var hashedPassword = Helper.HashPassword(newAdmin, model.Password);
        newAdmin.PasswordHash = hashedPassword;
        await _userRepository.AddAsync(newAdmin);
        return (model.Username, model.Password);
    }
    public async Task<List<UserDto>> GetAdminsAsync()
    {
        var users = await _redisService.GetUsersAsync(Constants.GetUsersAsyncCacheKey);
        if (users is not null)
        {
            var adminsCache = users.Where(x => x.Role == Constants.AdminRole).ToList();
            return adminsCache.ParseToDtos();
        }

        users = await _userRepository.GetAllAsync();
        var admins = users.Where(x => x.Role == Constants.AdminRole).ToList();
        await _redisService.SetUsersAsync(Constants.GetUsersAsyncCacheKey, users);
        return admins.ParseToDtos();
    }
    private async Task IsUniqueUsername(string username)
    {
        var users = await _redisService.GetUsersAsync(Constants.GetUsersAsyncCacheKey);
        if (users is not null)
        {
            var isUnique = users.Any(x => x.Username == username);
            if (isUnique) throw new Exception("Username must be unique");
        }
        users = await _userRepository.GetAllAsync();
        if (users == null)
            return;
        var isUniqueAdmin = users.Any(u => u.Username == username);
        if (isUniqueAdmin) throw new Exception("Username must be unique");
        await _redisService.SetUsersAsync(Constants.GetUsersAsyncCacheKey, users);
    }
}