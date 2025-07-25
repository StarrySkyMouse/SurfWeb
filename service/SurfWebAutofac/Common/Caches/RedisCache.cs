using System.Text.Json;
using Common.Caches.Base;
using StackExchange.Redis;

namespace Common.Caches;

public class RedisCache : ICache
{
    private readonly IDatabase _db;
    private readonly int _expirationMinute;

    public RedisCache(IConnectionMultiplexer connectionMultiplexer, int expirationMinute)
    {
        _db = connectionMultiplexer.GetDatabase();
        _expirationMinute = expirationMinute;
    }

    public bool Exists(string cacheKey, Action? action = null)
    {
        var exists = _db.KeyExists(cacheKey);
        if (!exists && action != null) action();
        return exists;
    }

    public T? Get<T>(string cacheKey, T? defaultValue = default)
    {
        var value = _db.StringGet(cacheKey);
        if (!value.HasValue)
            return defaultValue;
        return JsonSerializer.Deserialize<T>(value!);
    }

    public T? GetOrFunc<T>(string cacheKey, Func<T> func)
    {
        var value = _db.StringGet(cacheKey);
        if (value.HasValue) return JsonSerializer.Deserialize<T>(value!);
        var result = func();
        if (result != null) Set(cacheKey, result);
        return result;
    }

    public void Set<T>(string cacheKey, T value)
    {
        var json = JsonSerializer.Serialize(value);
        _db.StringSet(cacheKey, json, TimeSpan.FromMinutes(_expirationMinute));
    }

    public void Remove(string key)
    {
        _db.KeyDelete(key);
    }
}