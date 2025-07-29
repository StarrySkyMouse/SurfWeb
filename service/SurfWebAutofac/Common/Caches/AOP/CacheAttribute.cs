namespace Common.Caches.AOP;

/// <summary>
///     缓存标注特性,方法的
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class CacheAttribute : Attribute
{
    /// <summary>
    ///     缓存时间，单位秒
    /// </summary>
    public int CacheTime { get; set; } = -1;
}