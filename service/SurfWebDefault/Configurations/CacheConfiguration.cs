using Common.Caches;
using Model.Cahces;

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
        //注册缓存存储对象
        builder.Services.AddSingleton<DataCache>();
        //注册缓存管理对象
        builder.Services.AddScoped<CacheManage>();
    }
}