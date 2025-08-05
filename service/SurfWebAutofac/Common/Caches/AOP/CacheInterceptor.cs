using System.Text.Json;
using Castle.DynamicProxy;
using Common.Caches.Base;

namespace Common.Caches.AOP;
/// <summary>
/// 缓存拦截器
/// </summary>
public class CacheInterceptor : IInterceptor
{
    private readonly ICache _cache;

    public CacheInterceptor(ICache cache)
    {
        _cache = cache;
    }

    public void Intercept(IInvocation invocation)
    {
        // 获取当前被拦截的方法（接口或实现）
        var method = invocation.MethodInvocationTarget ?? invocation.Method;

        // 优先判断 NoCacheAttribute
        if (method.IsDefined(typeof(NoCacheAttribute), true) ||
            method.DeclaringType?.IsDefined(typeof(NoCacheAttribute), true) == true)
        {
            invocation.Proceed();
            return;
        }

        // 1. 先获取方法上的特性
        var cacheAttr = method.GetCustomAttributes(typeof(CacheAttribute), true)
            .FirstOrDefault() as CacheAttribute;

        // 2. 如果方法上没有，再获取类上的特性
        if (cacheAttr == null && method.DeclaringType != null)
            cacheAttr = method.DeclaringType.GetCustomAttributes(typeof(CacheAttribute), true)
                .FirstOrDefault() as CacheAttribute;
        if (cacheAttr == null)
        {
            // 没有缓存特性，直接执行原始方法
            invocation.Proceed();
            return;
        }

        // 3. 生成缓存 Key（参数用 Json 序列化更健壮）
        var argsKey = string.Join(",",
            invocation.Arguments.Select(a => a == null ? "<Null>" : JsonSerializer.Serialize(a)));
        var key = $"{method.DeclaringType?.FullName}.{method.Name}({argsKey})";
        // 4. 获取缓存值
        invocation.ReturnValue = _cache.GetOrFunc(key, () =>
        {
            invocation.Proceed();
            return invocation.ReturnValue;
        }, cacheAttr.CacheTime);
    }
}