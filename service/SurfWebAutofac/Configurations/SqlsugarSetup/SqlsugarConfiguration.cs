using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configurations.AutofacSetup.Register;
using Autofac.Core;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Configurations.SqlsugarSetup
{
    public static class SqlsugarConfiguration
    {
        public static void AddSqlsugarConfiguration(this WebApplicationBuilder builder)
        {
            // 获取主库和从库连接字符串
            var masterConn = configuration.GetConnectionString("Master");
            var slave1Conn = configuration.GetConnectionString("Slave1");
            var slave2Conn = configuration.GetConnectionString("Slave2");

            // 配置主从分离
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = masterConn,
                DbType = DbType.Sqlite, // 根据你的数据库类型选用
                IsAutoCloseConnection = true,
                // 主从分离配置
                SlaveConnectionConfigs = new List<SlaveConnectionConfig>
                {
                    new SlaveConnectionConfig { ConnectionString = slave1Conn, HitRate = 10 },
                    new SlaveConnectionConfig { ConnectionString = slave2Conn, HitRate = 5 }
                }
            });
            // 可选：AOP日志、错误处理等
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql); // 记录SQL
            };
            // 注入到 DI
            builder.Services.AddScoped<ISqlSugarClient>(_ => db);
        }
    }
}
