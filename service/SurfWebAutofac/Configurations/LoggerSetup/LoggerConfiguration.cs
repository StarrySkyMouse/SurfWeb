using Autofac.Core;
using Common.Logger;
using Configurations.LoggerSetup.Sink;
using Microsoft.AspNetCore.Builder;
using Serilog;
using System.Configuration;

namespace Configurations.LoggerSetup;

public static class LoggerConfiguration
{
    /// <summary>
    ///     日志配置
    /// </summary>
    /// <param name="builder"></param>
    public static void AddLoggerConfiguration(this WebApplicationBuilder builder)
    {
        //依赖注入
        builder.Services.AddLoggerService(builder.Configuration, builder.Host, (cfg, services) => cfg
            //Fluent API风格
            .AddDbLoggerSinkExecute<DbLoggerSinkExecute>(services)
            .AddServiceLoggerSinkExecute<ServiceLoggerSinkExecute>(services));
    }
}