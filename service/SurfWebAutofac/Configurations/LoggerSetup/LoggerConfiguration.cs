using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configurations.LoggerSetup.Sink;
using Configurations.SqlsugarSetup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;


namespace Configurations.LoggerSetup
{
    public static class LoggerConfiguration
    {
        private static Serilog.LoggerConfiguration WhereIf(this Serilog.LoggerConfiguration cfg, bool condition,
            Func<Serilog.LoggerConfiguration, Serilog.LoggerConfiguration> predicate)
        {
            if (!condition)
            {
                return cfg;
            }

            return predicate(cfg);
        }

        /// <summary>
        /// 日志配置
        /// </summary>
        /// <param name="builder"></param>
        public static void AddLoggerConfiguration(this WebApplicationBuilder builder)
        {
            var loggerConfig = builder.Configuration.GetSection("LoggerConfig").Get<LoggerConfig>();
            if (loggerConfig == null) throw new InvalidOperationException("日志配置LoggerConfig丢失");
            // 配置 Serilog
            //https://github.com/serilog/serilog/wiki
            Log.Logger = new Serilog.LoggerConfiguration()
                //读取配置文件中的Serilog配置，代码配置优先会合并配置（简化配置只做部分个性化设置）
                //.ReadFrom.Configuration(AppSettings.Configuration)
                //自动附加上下文信息
                .Enrich.FromLogContext()
                // 输出到控制台
                .WhereIf(loggerConfig.IsOpenConsole, cfg => cfg.WriteTo.Async(t => t.Console()))
                //文件输出
                .WhereIf(loggerConfig.FileConfig.IsOpen,
                    cfg => cfg.WriteTo.Async(t => t.File(loggerConfig.FileConfig.Path,
                        retainedFileCountLimit: loggerConfig.FileConfig.RetentionDay,
                        rollingInterval: RollingInterval.Day)))
                // 输出到 Seq
                .WhereIf(loggerConfig.SeqConfig.IsOpen,
                    cfg => cfg.WriteTo.Async(t => t.Seq(loggerConfig.SeqConfig.Url)))
                //输出到数据库
                .WhereIf(loggerConfig.IsOpenDb, cfg => cfg.WriteTo.Async(t => t.Sink<CustomDbSink>()))
                .CreateLogger();
            // 替换默认日志为 Serilog
            builder.Host.UseSerilog();
        }
    }
}
