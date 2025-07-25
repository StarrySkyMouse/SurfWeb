namespace Common.Caches.Base;

/// <summary>
///     缓存类型
/// </summary>
public enum CacheType
{
    Memory,
    Redis
}

/// <summary>
///     只能配置一种缓存
/// </summary>
public class CacheConfig
{
    /// <summary>
    ///     缓存模式
    /// </summary>
    public CacheType Type { get; set; } = CacheType.Memory;

    /// <summary>
    ///     Redis配置
    /// </summary>
    public CacheConfig_RedisConfig RedisConfig { get; set; }

    /// <summary>
    ///     缓存有效时间
    /// </summary>
    public int ExpirationMinute { get; set; } = 30;
}

public class CacheConfig_RedisConfig
{
    public string Connect { get; set; }
}