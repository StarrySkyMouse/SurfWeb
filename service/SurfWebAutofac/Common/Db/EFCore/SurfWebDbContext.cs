using System.Linq.Expressions;
using System.Reflection;
using Common.Db.Base;
using Common.Db.EFCore.CustomModelBuilder;
using Common.Db.EFCore.Seed;
using Microsoft.EntityFrameworkCore;

namespace Common.Db.EFCore;

/// <summary>
///     数据库连接上下文
/// </summary>
public class SurfWebDbContext : DbContext
{
    private readonly DbConfig _dbConfig;
    private readonly IEFCoreDbSeed _dbSeed;
    private readonly IEFCoreModelBuilder _modelBuilder;

    public SurfWebDbContext(DbConfig dbConfig, IEFCoreDbSeed dbSeed, IEFCoreModelBuilder modelBuilder)
    {
        _dbConfig = dbConfig;
        _dbSeed = dbSeed;
        _modelBuilder = modelBuilder;
    }
    /// <summary>
    ///     配置实体
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 加载 Model.dll 程序集
        var modelAssembly = Assembly.Load("Model");
        // 查找 Model.dll 中所有继承自 BaseEntity 的具体类
        var entityTypes = modelAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(BaseEntity).IsAssignableFrom(t));
        foreach (var type in entityTypes)
        {
            var entityBuilder = modelBuilder.Entity(type);
            // 设置表名为类名
            entityBuilder.ToTable(type.Name.Replace("Model", ""));
            // 设置主键  
            entityBuilder.HasKey("Id");
            // 表达式树构建 t => t.IsDelete == 0
            var param = Expression.Parameter(type, "t");
            var prop = Expression.Property(param, "IsDelete");
            var zero = Expression.Constant(0);
            var body = Expression.Equal(prop, zero);
            var lambda = Expression.Lambda(body, param);
            entityBuilder.HasQueryFilter(lambda);
        }

        _modelBuilder.OnModelCreating(modelBuilder);
    }

    /// <summary>
    ///     配置数据库连接
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        switch (_dbConfig.DbType)
        {
            case DbTypeEnum.Sqlite:
                optionsBuilder.UseSqlite(_dbConfig.DbConnection);
                break;
            case DbTypeEnum.MySQL:
                optionsBuilder.UseMySql(_dbConfig.DbConnection, ServerVersion.AutoDetect(_dbConfig.DbConnection));
                break;
        }

        ////数据生成
        ////弃用HasData
        ////官方更加推荐的方式 https://learn.microsoft.com/zh-cn/ef/core/what-is-new/ef-core-9.0/whatsnew#improved-data-seeding
        _dbSeed.Seed(optionsBuilder);
    }
}