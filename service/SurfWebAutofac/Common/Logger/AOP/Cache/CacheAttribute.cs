namespace Common.Logger.AOP.Cache;

/// <summary>
///     缓存标注特性,方法的，可继承的
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CacheAttribute : Attribute
{
    /// <summary>
    ///     缓存时间，单位秒
    /// </summary>
    public int CacheTime { get; set; } = 60;
}