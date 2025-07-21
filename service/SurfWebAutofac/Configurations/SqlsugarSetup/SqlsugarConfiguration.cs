using Autofac.Core;
using Configurations.SqlsugarSetup.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        // 配置主从分离
        var db = new SqlSugarClient(new ConnectionConfig
        {
            ConnectionString = sqlsugarConfig.MasterConn,
            DbType = sqlsugarConfig.DbType, // 根据你的数据库类型选用
            IsAutoCloseConnection = true,
            // 主从分离配置
            SlaveConnectionConfigs = sqlsugarConfig.SlaveConns.Select(slave => new SlaveConnectionConfig
            {
                HitRate = slave.HitRate, // 从库权重
                ConnectionString = slave.SlaveConn // 从库连接字符串
            }).ToList()
        });
        // 可选：AOP日志、错误处理等
        db.Aop.OnLogExecuting = (sql, pars) =>
        {
            Console.WriteLine(sql); // 记录SQL
        };
        // 注入到 DI
        builder.Services.AddScoped<ISqlSugarClient>(_ => db);
        //数据库初始化
        if (sqlsugarConfig.IsDataCreate)
        {
            builder.Services.AddHostedService<SeedDataHostedService>();
        }
    }
}