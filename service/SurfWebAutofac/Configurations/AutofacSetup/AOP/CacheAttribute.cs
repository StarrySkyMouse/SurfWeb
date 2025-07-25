namespace Configurations.AutofacSetup.AOP;

/// <summary>
///     缓存标注特性,方法的，可继承的
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
internal class CacheAttribute : Attribute
{
}