using Common.Caches.Base;
using Microsoft.AspNetCore.Http;

namespace Common.CustomMiddlewares.UniqueVisitor;

public class UniqueVisitorMiddleware
{
    private readonly ICache _cache;
    private readonly RequestDelegate _next;

    public UniqueVisitorMiddleware(RequestDelegate next, ICache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var today = DateTime.UtcNow.Date;
        var setKey = $"UniqueIPs_{today:yyyyMMdd}";
        var ip = context.Connection.RemoteIpAddress?.ToString();

        // 用HashSet存IP
        var ipSet = _cache.GetOrFunc(setKey, () => { return new HashSet<string>(); }, new Func<int>(() =>
        {
            //获取还有多少秒到24点
            var now = DateTime.Now;
            var tomorrow = now.Date.AddDays(1); // 明天0点
            var secondsLeft = (int)(tomorrow - now).TotalSeconds;
            return secondsLeft;
        })());
        lock (ipSet)
        {
            if (!ipSet.Contains(ip)) ipSet.Add(ip);
        }

        await _next(context);
    }
}