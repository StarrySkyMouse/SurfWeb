namespace Common.Caches.AOP;

/// <summary>
///     标注不使用缓存
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class NoCacheAttribute : Attribute
{
}