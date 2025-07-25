using Common.Logger;
using Configurations.LoggerSetup.Sink;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Configurations.LoggerSetup;

public static class LoggerConfiguration
{
    private static Serilog.LoggerConfiguration WhereIf(this Serilog.LoggerConfiguration cfg, bool condition,
        Func<Serilog.LoggerConfiguration, Serilog.LoggerConfiguration> predicate)
    {
        if (!condition) return cfg;

        return predicate(cfg);
    }

    /// <summary>
    ///     日志配置
    /// </summary>
    /// <param name="builder"></param>
    public static void AddLoggerConfiguration(this WebApplicationBuilder builder)
    {
        //依赖注入
        builder.Services.AddLoggerService(builder.Configuration)
            .AddDbLoggerSinkExecute<DbLoggerSinkExecute>();
        // 替换默认日志为 Serilog
        builder.Host.UseSerilog();
    }
}