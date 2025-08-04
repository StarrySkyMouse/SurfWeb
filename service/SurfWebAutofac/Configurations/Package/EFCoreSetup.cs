using Configurations.AutofacSetup;
using Configurations.AutoMapperSetup;
using Configurations.LoggerSetup;
using Configurations.QuartzSetup;
using Microsoft.AspNetCore.Builder;

namespace Configurations.Package;

public static class EFCoreSetup
{
    public static void AddEFCoreSetup(this WebApplicationBuilder builder)
    {
        //配置Autofac
        builder.AddEFCoreAutofacConfiguration();
        //配置WebApi
        builder.AddWebApiConfiguration();
        //配置数据库
        builder.AddEFCoreConfiguration();
        builder.AddDapperConfiguration();
        //配置缓存
        builder.AddCacheConfiguration();
        //配置日志
        builder.AddLoggerConfiguration();
        //配置限流
        builder.AddAspNetCoreRateLimitConfiguration();
        //配置AutoMapper
        builder.AddAutoMapperConfiguration();
        //定时任务
        builder.AddQuartzConfiguration();

        var app = builder.Build();

        //WebApi中间件
        app.UseAppMiddleware();
        //限流中间件
        app.UseAspNetCoreRateLimitMiddleware();

        app.Run();
    }
}