using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Db.Dapper;

public static class ServiceCollectionExtensions
{
    public static void AddDapperService(this IServiceCollection services, IConfiguration configuration)
    {
        var dapperConfig = configuration.GetSection("DapperConfig").Get<DapperConfig>();
        if (dapperConfig == null) throw new InvalidOperationException("数据库配置DapperConfig丢失");
        services.AddScoped<ISqlHelp>(_ => new MySqlHelp(dapperConfig.DbConnection));
    }
}