using Ec.Data.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ec.Service.In_memory_Storage;

public class RedisService(IConnectionMultiplexer connectionMultiplexer)
{
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();


    public void SetUser(string key, User user)
    {
        string value = JsonConvert.SerializeObject(user);
        _database.StringSet(key, value, TimeSpan.FromSeconds(10000));
    }
    public void SetUser(string key1, string key2, User user)
    {
        string value = JsonConvert.SerializeObject(user);
        if (string.IsNullOrEmpty(key1))
            _database.StringSet(key1, value, TimeSpan.FromSeconds(10000));

        if (string.IsNullOrEmpty(key2))
            _database.StringSet(key2, value, TimeSpan.FromSeconds(10000));
    }
    public User GetUser(string key)
    {
        string value = _database.StringGet(key);
        if (string.IsNullOrEmpty(value))
            return null;
        return JsonConvert.DeserializeObject<User>(value);
    }

}
