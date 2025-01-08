using Ec.Common.Constants;
using Ec.Data.Entities;
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

    public void SetSeller(string key, Dictionary<string, User> users, TimeSpan time)
    {
        var cacheEntiryOptions = new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = time
        };
        _memoryCache.Set(key, users, cacheEntiryOptions);
    }

    public User GetSeller(string phoneNumber)
    {
        var found = _memoryCache.TryGetValue(Constants.SellerKey, out Dictionary<string, User> sellers);
        if (!found)
            return null;
        foreach (var seller in sellers)
        {
            if(seller.Key == phoneNumber)
                return seller.Value;
        }
        return null;
    }

}
