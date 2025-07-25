using Common.Caches;
using Microsoft.AspNetCore.Builder;

namespace Configurations;

public static class CacheConfiguration
{
    /// <summary>
    ///     缓存
    /// </summary>
    /// <param name="builder"></param>
    public static void AddCacheConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddCachesService(builder.Configuration);
    }
}