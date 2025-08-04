using Common.Db.EFCore.CustomModelBuilder;
using Common.Db.EFCore.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Db.EFCore;

public static class ServiceCollectionExtensions
{
    public static void AddEFCoreService(this IServiceCollection services
        , Action<DbContextOptionsBuilder>? seedAction = null,
        Action<ModelBuilder>? modelBuilderAction = null)
    {
        //项目数据库  
        services.AddSingleton(sp =>
        {
            var dbConfig = sp.GetRequiredService<IConfiguration>().GetSection("EFCoreConfig").Get<DbConfig>();
            if (dbConfig == null) throw new InvalidOperationException("数据库配置EFCoreConfig部分缺失");
            return dbConfig;
        });
        if (seedAction != null)
            services.AddSingleton<IEFCoreDbSeed>(sp =>
            {
                var dbConfig = sp.GetRequiredService<IConfiguration>().GetSection("EFCoreConfig").Get<DbConfig>();
                if (dbConfig == null) throw new InvalidOperationException("数据库配置EFCoreConfig部分缺失");
                if (dbConfig.IsDataCreate) return new EFCoreDbSeed(seedAction);

                return new EFCoreDbSeedNull();
            });
        services.AddSingleton<IEFCoreModelBuilder>(_ =>
        {
            if (modelBuilderAction != null) return new EFCoreModelBuilder(modelBuilderAction);
            return new EFCoreModelBuilderNull();
        });
        // 注册数据库配置和EFCore  
        services.AddDbContext<SurfWebDbContext>();
    }
}