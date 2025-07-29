using Castle.DynamicProxy;
using Common.Caches.Base;

namespace Common.Caches.AOP;

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
        if (method.GetCustomAttributes(typeof(CacheAttribute), true)
                .FirstOrDefault() is NoCacheAttribute)
        {
            invocation.Proceed();
            return;
        }

        var declaringType = method.DeclaringType;
        // 1. 先获取方法上的特性
        var methodCacheAttr = method.GetCustomAttributes(typeof(CacheAttribute), true)
            .FirstOrDefault() as CacheAttribute;

        var cacheAttr = methodCacheAttr;
        // 2. 如果方法上没有，再获取类上的特性
        if (cacheAttr == null)
            cacheAttr = declaringType.GetCustomAttributes(typeof(CacheAttribute), true)
                .FirstOrDefault() as CacheAttribute;

        if (cacheAttr == null)
        {
            // 没有缓存特性，直接执行原始方法
            invocation.Proceed();
            return;
        }

        // 3. 生成缓存 Key
        var key =
            $"{declaringType.FullName}.{method.Name}({string.Join(",", invocation.Arguments.Select(a => a?.ToString() ?? "<Null>"))})";

        // 4. 获取缓存值
        invocation.ReturnValue = _cache.GetOrFunc(key, () =>
        {
            invocation.Proceed();
            return invocation.ReturnValue;
        }, cacheAttr.CacheTime);
    }
}