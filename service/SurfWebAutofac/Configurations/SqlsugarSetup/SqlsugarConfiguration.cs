using Common.Logger.Sign;
using Configurations.SqlsugarSetup.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository.BASE.Log.SugarClient;
using Repository.BASE.MainSqlSugar.SugarClient;
using SqlSugar;

namespace Configurations.SqlsugarSetup;

/// <summary>
///     数据库配置
/// </summary>
public static class SqlsugarConfiguration
{
    public static void AddSqlsugarConfiguration(this WebApplicationBuilder builder)
    {
        var sqlsugarConfig = builder.Configuration.GetSection("SqlsugarConfig").Get<SqlsugarConfig>();
        if (sqlsugarConfig == null) throw new InvalidOperationException("数据库配置SqlsugarConfig丢失");
        //应用数据库
        builder.Services.AddScoped<IMainSqlSugarClient>(sp =>
        {
            var connectionConfig = new ConnectionConfig
            {
                DbType = sqlsugarConfig.DbType,
                IsAutoCloseConnection = true,
                ConnectionString = sqlsugarConfig.MainConfig.DbConnection
            };
            //id生成器配置
            SnowFlakeSingle.WorkId = sqlsugarConfig.SnowflakeIdConfig.WorkId;
            SnowFlakeSingle.DatacenterId = sqlsugarConfig.SnowflakeIdConfig.DatacenterId;
            //主从数据库配置
            if (sqlsugarConfig.MainConfig.IsOpenSlave && sqlsugarConfig.MainConfig.SlaveConfigs.Any(t => t.IsEnable))
                connectionConfig.SlaveConnectionConfigs = sqlsugarConfig.MainConfig.SlaveConfigs
                    .Where(t => t.IsEnable)
                    .Select(slave => new SlaveConnectionConfig
                    {
                        HitRate = slave.HitRate, // 从库权重
                        ConnectionString = slave.DbConnection // 从库连接字符串
                    }).ToList();
            var db = new MainSqlSugarClient(connectionConfig);

            #region AOP

            ////SQL执行前
            //db.Aop.OnLogExecuting = (sql, pars) =>
            //{
            //    //获取原生SQL推荐 5.1.4.63  性能OK
            //    UtilMethods.GetNativeSql(sql, pars);
            //};
            //SQL执行后
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                var logger = sp.GetRequiredService<ILogger<IDbLoggerSign>>();
                logger.LogInformation(db.Ado.SqlExecutionTime.ToString());
            };
            //SQL报错
            db.Aop.OnError = exp =>
            {
                var logger = sp.GetRequiredService<ILogger<IDbLoggerSign>>();
                logger.LogError(exp, "SqlSugar 执行出错: {Sql}", exp.Sql);
            };

            #endregion

            return db;
        });
        //日志数据库
        builder.Services.AddSingleton<ILogSqlSugarClient>(sp =>
        {
            var db = new LogSqlSugarClient(new ConnectionConfig
            {
                DbType = sqlsugarConfig.DbType,
                IsAutoCloseConnection = true,
                ConnectionString = sqlsugarConfig.LogConfig.DbConnection
            });
            //SQL报错
            db.Aop.OnError = exp =>
            {
                var logger = sp.GetRequiredService<ILogger<IDbLoggerSign>>();
                logger.LogError(exp, "SqlSugar 执行出错: {Sql}", exp.Sql);
            };
            return db;
        });
        //数据库初始化
        if (sqlsugarConfig.IsDataCreate) builder.Services.AddHostedService<SeedDataHostedService>();
    }
}