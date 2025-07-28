using Autofac.Core;
using Common.Logger.Sign;
using Common.Logger.Sink;
using Common.Quartz.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Filters;

namespace Common.Logger;

/// <summary>
///     链式调用+定制扩展+对外依赖注入能力
/// </summary>
public static class ServiceCollectionExtensions
{
    private static LoggerConfiguration WhereIf(this LoggerConfiguration cfg, bool condition,
        Func<LoggerConfiguration, LoggerConfiguration> predicate)
    {
        if (!condition) return cfg;
        return predicate(cfg);
    }
    public static void AddLoggerService(this IServiceCollection services, IConfiguration configuration,
        ConfigureHostBuilder hostBuilder, Action<LoggerConfiguration, IServiceProvider> loggerCfg)
    {
        var loggerConfig = configuration.GetSection("LoggerConfig").Get<LoggerConfig>();
        if (loggerConfig == null) throw new InvalidOperationException("日志配置LoggerConfig丢失");
        // 配置 Serilog
        //https://github.com/serilog/serilog/wiki
        Log.Logger = new LoggerConfiguration()
            //读取配置文件中的Serilog配置，代码配置优先会合并配置（简化配置只做部分个性化设置）
            //.ReadFrom.Configuration(AppSettings.Configuration)
            //自动附加上下文信息
            .Enrich.FromLogContext()
            // 输出到控制台
            .WriteTo.Logger(t => t
                .Filter.ByIncludingOnly(Matching.FromSource(typeof(IConsoleLoggerSign).FullName))
                .WriteTo.Async(a => a.Console()))
            //文件输出
            .WriteTo.Logger(t => t
                .Filter.ByIncludingOnly(Matching.FromSource(typeof(IFileLoggerSign).FullName))
                .WriteTo.Async(a => a.File(loggerConfig.FileConfig.Path,
                    retainedFileCountLimit: loggerConfig.FileConfig.RetentionDay,
                    rollingInterval: RollingInterval.Day)))
            //输出到 Seq
            .WhereIf(!string.IsNullOrWhiteSpace(loggerConfig.SeqConfig.Url), cfg => cfg
                .WriteTo.Logger(t => t
                    .Filter.ByIncludingOnly(Matching.FromSource(typeof(IFileLoggerSign).FullName))
                    .WriteTo.Async(a => a.Seq(loggerConfig.SeqConfig.Url))))
            .CreateLogger();
        // 替换默认日志为 Serilog
        hostBuilder.UseSerilog((context, services, configuration) =>
        {
            //Sink不支持构造函数注入，在Buider之后DI和Autofac容器合并后在传递容器
            //Buider之前拿不到IDbLogServices对象
            loggerCfg(configuration, services);
        });
    }
    /// <summary>
    /// 依赖注入实现
    /// </summary>
    public static LoggerConfiguration AddDbLoggerSinkExecute<T>(this LoggerConfiguration configuration,
        IServiceProvider services) where T : IDbLoggerSink
    {
        configuration.WriteTo.Logger(t => t
            .Filter.ByIncludingOnly(Matching.FromSource(typeof(IDbLoggerSign).FullName))
            .WriteTo.Async(a => a.Sink((IDbLoggerSink)Activator.CreateInstance(typeof(T), services))));
        return configuration;
    }
    public static LoggerConfiguration AddServiceLoggerSinkExecute<T>(this LoggerConfiguration configuration,
        IServiceProvider services) where T : IServiceLoggerSink
    {
        configuration.WriteTo.Logger(t => t
            .Filter.ByIncludingOnly(Matching.FromSource(typeof(IServiceLoggerSign).FullName))
            .WriteTo.Async(a => a.Sink((IServiceLoggerSink)Activator.CreateInstance(typeof(T), services))));
        return configuration;
    }
}