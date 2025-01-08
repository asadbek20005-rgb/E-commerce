using Ec.Data.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json.Serialization;

namespace Ec.Service.In_memory_Storage;

public class RedisService(IConnectionMultiplexer connectionMultiplexer)
{
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();


    public void SetUser(string key, User user)
    {
        string value = JsonConvert.SerializeObject(user);
        _database.StringSet(key, value);
    }

    public User GetUser(string key)
    {
        string value = _database.StringGet(key);
        if (string.IsNullOrEmpty(value))
            return null;
        return JsonConvert.DeserializeObject<User>(value);
    }

}
