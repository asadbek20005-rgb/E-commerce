using Ec.Data.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ec.Service.In_memory_Storage;

public class RedisService(IConnectionMultiplexer connectionMultiplexer)
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    public async Task SetUser(string key, User user)
    {
        string value = JsonConvert.SerializeObject(user);
        await _database.StringSetAsync(key, value, TimeSpan.FromSeconds(10000));
    }
    public async Task SetUsersAsync(string key, List<User> users)
    {
        string value = JsonConvert.SerializeObject(users);
        await _database.StringSetAsync(key, value);
    }
    public async Task<User> GetUser(string key)
    {
        string value = await _database.StringGetAsync(key);
        if (string.IsNullOrEmpty(value))
            return null;
        return JsonConvert.DeserializeObject<User>(value);
    }
    public async Task<List<User>> GetUsersAsync(string key)
    {
        string value = await _database.StringGetAsync(key);
        if (string.IsNullOrEmpty(value))
            return null;
        return JsonConvert.DeserializeObject<List<User>>(value);
    }
}
