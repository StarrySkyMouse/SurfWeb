using Common.Caches.Base;
using Microsoft.Extensions.Caching.Memory;

namespace Common.Caches;

public class MemoryCache : ICache
{
    //秒
    private readonly int _defaultCacheTime;
    private readonly IMemoryCache _memoryCache;

    public MemoryCache(IMemoryCache memoryCache, int defaultCacheTime)
    {
        _memoryCache = memoryCache;
        _defaultCacheTime = defaultCacheTime;
    }

    public bool Exists(string cacheKey, Action? action = null)
    {
        var result = _memoryCache.TryGetValue(cacheKey, out _);
        if (!result && action != null) action.Invoke();
        return result;
    }

    public T? Get<T>(string cacheKey, T? defaultValue = default)
    {
        var result = _memoryCache.Get<T>(cacheKey);
        if (result == null && defaultValue != null) return defaultValue;

        return result;
    }

    public T? GetOrFunc<T>(string cacheKey, Func<T> func, int cacheTime = -1)
    {
        var result = _memoryCache.Get<T>(cacheKey);
        if (result == null || EqualityComparer<T>.Default.Equals(result, default))
        {
            result = func();
            Set(cacheKey, result, cacheTime);
        }

        return result;
    }

    public void Set<T>(string cacheKey, T value, int cacheTime = -1)
    {
        _memoryCache.Set(cacheKey, value, TimeSpan.FromSeconds(cacheTime != -1 ? cacheTime : _defaultCacheTime));
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}