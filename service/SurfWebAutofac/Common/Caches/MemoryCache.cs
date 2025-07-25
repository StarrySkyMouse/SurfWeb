using Common.Caches.Base;
using Microsoft.Extensions.Caching.Memory;

namespace Common.Caches;

public class MemoryCache : ICache
{
    private readonly int _expirationMinute;
    private readonly IMemoryCache _memoryCache;

    public MemoryCache(IMemoryCache memoryCache, int expirationMinute)
    {
        _memoryCache = memoryCache;
        _expirationMinute = expirationMinute;
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

    public T? GetOrFunc<T>(string cacheKey, Func<T> func)
    {
        var result = _memoryCache.Get<T>(cacheKey);
        if (result == null || EqualityComparer<T>.Default.Equals(result, default))
        {
            result = func();
            Set(cacheKey, result);
        }

        return result;
    }

    public void Set<T>(string cacheKey, T value)
    {
        _memoryCache.Set(cacheKey, value, TimeSpan.FromMinutes(_expirationMinute));
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}