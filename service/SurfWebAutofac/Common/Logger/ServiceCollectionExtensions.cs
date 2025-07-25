using Common.Logger.Sign;
using Common.Logger.Sink;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Common.Logger;

public static class ServiceCollectionExtensions
{
    private static LoggerConfiguration WhereIf(this LoggerConfiguration cfg, bool condition,
        Func<LoggerConfiguration, LoggerConfiguration> predicate)
    {
        if (!condition) return cfg;
        return predicate(cfg);
    }

    public static ILoggerConfigure AddLoggerService(this IServiceCollection services, IConfiguration configuration)
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
            .WhereIf(loggerConfig.IsOpenConsole, cfg => cfg.WriteTo.Async(t => t.Console())
                .Filter.ByIncludingOnly(t =>
                    t.Properties.ContainsKey("SourceContext") && t.Properties["SourceContext"].ToString() ==
                    $"\"{typeof(IConsoleLoggerSign).FullName}\""))
            //文件输出
            .WhereIf(loggerConfig.FileConfig.IsOpen,
                cfg => cfg.WriteTo.Async(t => t.File(loggerConfig.FileConfig.Path,
                        retainedFileCountLimit: loggerConfig.FileConfig.RetentionDay,
                        rollingInterval: RollingInterval.Day))
                    .Filter.ByIncludingOnly(t =>
                        t.Properties.ContainsKey("SourceContext") && t.Properties["SourceContext"].ToString() ==
                        $"\"{typeof(IFileLoggerSign).FullName}\""))
            // 输出到 Seq
            .WhereIf(loggerConfig.SeqConfig.IsOpen,
                cfg => cfg.WriteTo.Async(t => t.Seq(loggerConfig.SeqConfig.Url))
                    .Filter.ByIncludingOnly(t =>
                        t.Properties.ContainsKey("SourceContext") && t.Properties["SourceContext"].ToString() ==
                        $"\"{typeof(ISeqLoggerSign).FullName}\""))
            //输出到数据库
            .WhereIf(loggerConfig.IsOpenDb, cfg => cfg.WriteTo.Async(t => t.Sink<DbLoggerSink>())
                .Filter.ByIncludingOnly(t =>
                    t.Properties.ContainsKey("SourceContext") && t.Properties["SourceContext"].ToString() ==
                    $"\"{typeof(IDbLoggerSign).FullName}\""))
            .CreateLogger();
        return (ILoggerConfigure)services;
    }

    /// <summary>
    /// 依赖注入实现
    /// </summary>
    public static void AddDbLoggerSinkExecute<T>(this ILoggerConfigure services) where T : class, IDbLoggerSinkExecute
    {
        services.AddSingleton<IDbLoggerSinkExecute, T>();
    }
}

public interface ILoggerConfigure : IServiceCollection
{
}