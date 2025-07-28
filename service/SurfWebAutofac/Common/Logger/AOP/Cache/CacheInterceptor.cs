using Castle.DynamicProxy;
using Common.Caches.Base;

namespace Common.Logger.AOP.Cache;

public class CacheInterceptor : IInterceptor
{
    private readonly ICache _cache;

    public CacheInterceptor(ICache cache)
    {
        _cache = cache;
    }

    public void Intercept(IInvocation invocation)
    {
        // 1. 获取当前被拦截的方法（接口或实现）
        var method = invocation.MethodInvocationTarget ?? invocation.Method;
        //判断特性
        var cacheAttr = method.GetCustomAttributes(typeof(CacheAttribute), true).FirstOrDefault() as CacheAttribute;
        if (cacheAttr == null)
        {
            //执行原始方法
            invocation.Proceed();
            return;
        }

        // 生成缓存Key，根据方法名+参数拼接
        var key =
            $"{method.DeclaringType.FullName}.{method.Name}({string.Join(",", invocation.Arguments.Select(a => a?.ToString() ?? "<Null>"))})";
        // 获取缓存值
        invocation.ReturnValue = _cache.GetOrFunc(key, () =>
        {
            //缓存不存在则执行方法
            invocation.Proceed();
            return invocation.ReturnValue;
        });
    }
}