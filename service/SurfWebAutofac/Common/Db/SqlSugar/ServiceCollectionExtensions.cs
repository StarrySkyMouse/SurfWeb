using Common.Db.SqlSugar.Repository.Log.SugarClient;
using Common.Db.SqlSugar.Repository.Main.SugarClient;
using Common.Db.SqlSugar.Seed;
using Common.Logger.Dto;
using Common.Logger.Sign;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Common.Db.SqlSugar;

public static class ServiceCollectionExtensions
{
    public static void AddSqlSugarService(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlsugarConfig = configuration.GetSection("SqlsugarConfig").Get<SqlsugarConfig>();
        if (sqlsugarConfig == null) throw new InvalidOperationException("数据库配置SqlsugarConfig丢失");
        //id生成器配置
        SnowFlakeSingle.WorkId = sqlsugarConfig.SnowflakeIdConfig.WorkId;
        SnowFlakeSingle.DatacenterId = sqlsugarConfig.SnowflakeIdConfig.DatacenterId;
        //应用数据库
        services.AddScoped<IMainSqlSugarClient>(sp =>
        {
            var connectionConfig = new ConnectionConfig
            {
                DbType = sqlsugarConfig.DbType,
                IsAutoCloseConnection = true,
                ConnectionString = sqlsugarConfig.MainConfig.DbConnection
            };
            //主从数据库配置
            if (sqlsugarConfig.MainConfig.IsOpenSlave &&
                sqlsugarConfig.MainConfig.SlaveConfigs.Any(t => t.IsEnable))
                connectionConfig.SlaveConnectionConfigs = sqlsugarConfig.MainConfig.SlaveConfigs
                    .Where(t => t.IsEnable)
                    .Select(slave => new SlaveConnectionConfig
                    {
                        HitRate = slave.HitRate, // 从库权重
                        ConnectionString = slave.DbConnection // 从库连接字符串
                    }).ToList();
            var db = new MainSqlSugarClient(connectionConfig);

            #region AOP

            //SQL执行后
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                var logger = sp.GetRequiredService<ILogger<IDbLoggerSign>>();
                var paramStr = string.Join(", ", pars.Select(p => $"{p.ParameterName}={p.Value}"));
                logger.LogInformation(DbLoggerDto.InfoToString(
                    sp.GetRequiredService<IHttpContextAccessor>().HttpContext.TraceIdentifier, DbLoggerDtoType.Info,
                    $"{sql}; params: {paramStr}", db.Ado.SqlExecutionTime));
            };
            //SQL报错
            db.Aop.OnError = exp =>
            {
                //var logger = sp.GetRequiredService<ILogger<IDbLoggerSign>>();
                //logger.LogError(DbLoggerDto.ErrorToString(DbLoggerDtoType.Error, exp.Sql, db.Ado.SqlExecutionTime, exp.ToString()));
            };

            #endregion

            return db;
        });
        //日志数据库
        services.AddScoped<ILogSqlSugarClient>(sp =>
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
        if (sqlsugarConfig.IsDataCreate) services.AddHostedService<SeedDataHostedService>();
    }
}