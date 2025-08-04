using System.Reflection;
using Common.Db.Base;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Common.Db.SqlSugar.Seed;

public class SeedDataHostedService : IHostedService
{
    private readonly ILogger<SeedDataHostedService> _logger;
    private readonly ISqlSugarClient _sqlSugarClient;

    public SeedDataHostedService(ILogger<SeedDataHostedService> logger, ISqlSugarClient sqlSugarClient)
    {
        _logger = logger;
        _sqlSugarClient = sqlSugarClient;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            //创建数据库  
            _sqlSugarClient.DbMaintenance.CreateDatabase();
            var baseType = typeof(BaseEntity);
            //创建表
            _sqlSugarClient.CodeFirst.InitTables(
                //扫描所有继承自 BaseEntity 的类
                Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "Model.dll"))
                    .GetTypes()
                    .Where(t => t is { IsClass: true, IsAbstract: false } && baseType.IsAssignableFrom(t))
                    .ToArray()
            );
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SeedDataHostedService异常");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stop SeedDataHostedService");
        return Task.CompletedTask;
    }
}