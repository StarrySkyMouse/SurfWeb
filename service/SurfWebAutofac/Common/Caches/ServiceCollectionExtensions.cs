using Common.Caches.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Common.Caches;

public static class ServiceCollectionExtensions
{
    public static void AddCachesService(this IServiceCollection services,
        IConfiguration configuration)
    {
        var cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>() ?? new CacheConfig();

        switch (cacheConfig.Type)
        {
            case CacheType.Memory:
                services.AddMemoryCache();
                services.AddSingleton<ICache>(sp =>
                    new MemoryCache(sp.GetRequiredService<IMemoryCache>(),
                        cacheConfig.ExpirationMinute));
                break;
            case CacheType.Redis:
                services.AddSingleton<ICache>(sp =>
                    new RedisCache(ConnectionMultiplexer.Connect(cacheConfig.RedisConfig.Connect),
                        cacheConfig.ExpirationMinute));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}