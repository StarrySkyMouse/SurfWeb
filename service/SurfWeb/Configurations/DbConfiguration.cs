using Repositories;
using Repositories.Exterior;

namespace Configurations;

/// <summary>
/// </summary>
public static class DbConfiguration
{
    /// <summary>
    ///     配置数据库相关服务
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDbConfiguration(this WebApplicationBuilder builder)
    {
        //项目数据库  
        builder.Services.AddSingleton(sp =>
        {
            var dbConfig = sp.GetRequiredService<IConfiguration>().GetSection("DbConfig").Get<DbConfig>();
            if (dbConfig == null) throw new InvalidOperationException("数据库配置DbConfig部分缺失");
            return dbConfig;
        });
        //腐竹数据  
        builder.Services.AddSingleton(sp =>
        {
            var dbConfig = sp.GetRequiredService<IConfiguration>().GetSection("DataSourceConfig")
                .Get<DataSourceConfig>();
            if (dbConfig == null) throw new InvalidOperationException("数据库配置DataSourceConfig部分缺失");
            return dbConfig;
        });
        // 注册数据库配置和EFCore  
        builder.Services.AddDbContext<SurfWebDbContext>();
        builder.Services.AddSingleton<ISqlHelp, MySqlHelp>();
    }
}