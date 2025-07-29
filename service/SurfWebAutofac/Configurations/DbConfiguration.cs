using Common.Dapper;
using Common.SqlSugar;
using Microsoft.AspNetCore.Builder;

namespace Configurations;

/// <summary>
///     数据库配置
/// </summary>
public static class DbConfiguration
{
    public static void AddDbConfiguration(this WebApplicationBuilder builder)
    {
        //添加SqlSugar服务
        builder.Services.AddSqlSugarService(builder.Configuration);
        //外部其他数据库
        builder.Services.AddDapperService(builder.Configuration);
    }
}