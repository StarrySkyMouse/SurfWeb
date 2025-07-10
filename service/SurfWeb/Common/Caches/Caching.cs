using Microsoft.Extensions.Caching.Memory;

namespace Common.Caches;

public class Caching : ICaching
{
    private readonly IMemoryCache _cache;
    private readonly CachingConfig _cachingConfig;
    public Caching(IMemoryCache cache, CachingConfig cachingConfig)
    {
        _cache = cache;
        _cachingConfig = cachingConfig;
    }
    public bool Exists(string cacheKey)
    {
        return _cache.TryGetValue(cacheKey, out _);
    }
    public T? Get<T>(string cacheKey) where T : class
    {
        return _cache.Get(cacheKey) as T;
    }
    public void Remove(string key)
    {
        _cache.Remove(key);
    }
    public void Set<T>(string cacheKey, T value)
    {
        _cache.Set(cacheKey, value, new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cachingConfig.ExpirationTime)
        });
    }
    public T GetOrFunction<T>(string cacheKey, Func<T> func) where T : class
    {
        var value = Get<T>(cacheKey);
        if (value == null)
        {
            value = func();
            Set(cacheKey, value);
        }
        return value;
    }
}