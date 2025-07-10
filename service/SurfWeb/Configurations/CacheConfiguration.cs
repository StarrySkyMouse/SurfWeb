using Services.Caches;

namespace Configurations;

/// <summary>
/// </summary>
public static class CacheConfiguration
{
    /// <summary>
    ///     缓存配置
    /// </summary>
    /// <param name="builder"></param>
    public static void AddCacheConfiguration(this WebApplicationBuilder builder)
    {
        //注册缓存
        builder.Services.AddSingleton<DataCache>();
    }
}