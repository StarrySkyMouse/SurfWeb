﻿using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Configurations;

public static class AspNetCoreRateLimitConfiguration
{
    /// <summary>
    ///     配置Web API相关服务
    /// </summary>
    public static void AddAspNetCoreRateLimitConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions();
        builder.Services.AddMemoryCache();
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddInMemoryRateLimiting();
    }

    /// <summary>
    ///     中间件
    /// </summary>
    public static void UseAspNetCoreRateLimitMiddleware(this WebApplication app)
    {
        app.UseIpRateLimiting();
    }
}