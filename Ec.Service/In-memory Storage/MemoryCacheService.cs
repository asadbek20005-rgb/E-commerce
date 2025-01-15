using Microsoft.Extensions.Caching.Memory;

namespace Ec.Service.MemoryCache;

public class MemoryCacheService(IMemoryCache memoryCache)
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    public void Delete(string key)
    {
        _memoryCache.Remove(key);
    }

    public int GetCode(string key)
    {
        _ = _memoryCache.TryGetValue(key,
                                     value: out int value);
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan time)
    {
        var cacheEntiryOptions = new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = time
        };
        _memoryCache.Set(key, value, cacheEntiryOptions);
    }

    public void Set(string key, object value)
    {
        _memoryCache.Set(key, value);
    }

    public object Get(string key)
    {
        object value = _memoryCache.Get(key);
        return value;
    }

}
