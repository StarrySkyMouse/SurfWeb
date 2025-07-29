using AspNetCoreRateLimit;
using Common.Middlewares.ApiResponse;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.IpRateLimit
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIpRateLimitService(this IServiceCollection services, IConfiguration configuration)
        {
            // 2. 注册配置绑定相关服务
            services.AddOptions();
            // 3. 注册内存缓存（用于存储限流策略和计数器）
            services.AddMemoryCache();
            // 4. 绑定配置节到 IpRateLimitOptions
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            // 5. 注册策略存储和计数存储为内存实现
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            // 6. 注册限流配置
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            // 7. 注册限流处理策略（可选）
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }
        /// <summary>
        ///     添加Api响应中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseIpRateLimit(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 429)
                {
                    context.Response.Clear(); // 清空之前的内容
                    // 重置响应
                    context.Response.ContentType = "application/json";
                    var resp = "请求过于频繁，请1分钟后再试。";
                    var json = System.Text.Json.JsonSerializer.Serialize(resp);
                    await context.Response.BodyWriter.WriteAsync(System.Text.Encoding.UTF8.GetBytes(json));
                }
            });
            app.UseIpRateLimiting();
        }
    }
}
