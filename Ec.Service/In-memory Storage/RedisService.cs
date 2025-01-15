using Ec.Common.Constants;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Ec.Service.In_memory_Storage;

public class RedisService(IConnectionMultiplexer connectionMultiplexer)
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    public async Task Set(string key, object value)
    {
        RedisValue redisValue = JsonConvert.SerializeObject(value);
        await _database.StringSetAsync(key, redisValue, Constants.MemoryExpirationTime);
    }

    public async Task<RedisValue> Get(string key)
    {
        RedisValue redisValue = await _database.StringGetAsync(key);
        return redisValue;
    }

    public async Task<RedisValue> UpdateAndGet(string key, object value)
    {
        RedisValue redisValue = JsonConvert.SerializeObject(value);
        await _database.StringSetAsync(key, redisValue, Constants.MemoryExpirationTime);

        redisValue = await _database.StringGetAsync(key);
        return redisValue;
    }









}
