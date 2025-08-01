using System.Linq.Expressions;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Model.Entitys;
using Model.Entitys.Base;

namespace Repositories;

/// <summary>
///     数据库连接上下文
/// </summary>
public class SurfWebDbContext : DbContext
{
    private readonly DbConfig _dbConfig;

    public SurfWebDbContext(DbConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    /// <summary>
    ///     配置实体
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 获取所有继承自BasicEntity的类型
        var entityTypes = typeof(BaseEntity).Assembly.GetTypes()
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

        modelBuilder
            .Entity<PlayerCompleteModel>()
            .HasQueryFilter(t => t.Hide == false);
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

        //数据生成
        //弃用HasData
        //官方更加推荐的方式 https://learn.microsoft.com/zh-cn/ef/core/what-is-new/ef-core-9.0/whatsnew#improved-data-seeding
        if (_dbConfig.IsDataCreate)
            optionsBuilder.UseSeeding((context, _) =>
            {
                //清除所有数据
                context.Set<MapModel>().RemoveRange(context.Set<MapModel>());
                context.Set<NewRecordModel>().RemoveRange(context.Set<NewRecordModel>());
                context.Set<PlayerCompleteModel>().RemoveRange(context.Set<PlayerCompleteModel>());
                context.Set<PlayerModel>().RemoveRange(context.Set<PlayerModel>());
                context.Set<RankingModel>().RemoveRange(context.Set<RankingModel>());
                context.SaveChanges();
                //地图
                var mapId = 0;
                var mapDifficulty = new List<string> { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T0" };
                var fakerMapModel = new Faker<MapModel>()
                    .RuleFor(m => m.Id, f => mapId++.ToString())
                    .RuleFor(m => m.Name, f => "surf_" + f.Address.City())
                    .RuleFor(m => m.Difficulty, f => f.PickRandom(mapDifficulty))
                    .RuleFor(m => m.Img, f => f.Image.PicsumUrl())
                    .RuleFor(m => m.SurcessNumber, f => f.Random.Int(0, 100))
                    .RuleFor(m => m.BonusNumber, f => f.Random.Int(0, 5))
                    .RuleFor(m => m.StageNumber, f => f.Random.Int(0, 5))
                    .RuleFor(m => m.CreateTime, f => DateTime.Now)
                    .RuleFor(m => m.UpDateTime, f => DateTime.Now)
                    .RuleFor(m => m.IsDelete, f => 0)
                    .Generate(100);
                context.Set<MapModel>().AddRange(fakerMapModel);
                //玩家
                var playerId = 0;
                var fakerPlayerModel = new Faker<PlayerModel>()
                    .RuleFor(m => m.Id, f => playerId++.ToString())
                    .RuleFor(m => m.Name, f => f.Name.FindName())
                    .RuleFor(m => m.Integral, f => f.Random.Decimal(100, 5000))
                    .RuleFor(m => m.SucceesNumber, f => f.Random.Int(0, 100))
                    .RuleFor(m => m.WRNumber, f => f.Random.Int(0, 100))
                    .RuleFor(m => m.BWRNumber, f => f.Random.Int(0, 100))
                    .RuleFor(m => m.SWRNumber, f => f.Random.Int(0, 100))
                    .RuleFor(m => m.CreateTime, f => DateTime.Now)
                    .RuleFor(m => m.UpDateTime, f => DateTime.Now)
                    .RuleFor(m => m.IsDelete, f => 0)
                    .Generate(100);
                context.Set<PlayerModel>().AddRange(fakerPlayerModel);
                //排行
                var rankingId = 1;
                var rankingNumber = 1;
                var fakerRankingModel = new Faker<RankingModel>()
                    .RuleFor(m => m.Id, f => rankingId++.ToString())
                    .RuleFor(m => m.Type, f =>
                    {
                        if (rankingId <= 10) return RankingTypeEnum.Integral;

                        if (rankingId <= 20) return RankingTypeEnum.MainWR;

                        if (rankingId <= 30) return RankingTypeEnum.BountyWR;

                        return RankingTypeEnum.StageWR;
                    })
                    .RuleFor(m => m.Rank, f => rankingNumber++ % 10)
                    .RuleFor(m => m.PlayerId, f => f.Random.Int(0, 99).ToString())
                    //查询上面生成的玩家名称
                    .RuleFor(m => m.PlayerName, (f, m) => fakerPlayerModel.Where(t => t.Id == m.PlayerId).First().Name)
                    .RuleFor(m => m.Value, f => f.Random.Int(0, 100))
                    .RuleFor(m => m.CreateTime, f => DateTime.Now)
                    .RuleFor(m => m.UpDateTime, f => DateTime.Now)
                    .RuleFor(m => m.IsDelete, f => 0)
                    .Generate(40);
                context.Set<RankingModel>().AddRange(fakerRankingModel);
                //新的
                var newRecordId = 1;
                var fakerNewRecordModel = new Faker<NewRecordModel>()
                    .RuleFor(m => m.Id, f => newRecordId++.ToString())
                    .RuleFor(m => m.PlayerId, f => f.Random.Int(0, 99).ToString())
                    .RuleFor(m => m.PlayerName, (f, m) => fakerPlayerModel.Where(t => t.Id == m.PlayerId).First().Name)
                    .RuleFor(m => m.MapId, f => f.Random.Int(0, 99).ToString())
                    .RuleFor(m => m.MapName, (f, m) => fakerMapModel.Where(t => t.Id == m.MapId).First().Name)
                    .RuleFor(m => m.Type, f =>
                    {
                        if (newRecordId <= 10) return RecordTypeEnum.Main;

                        if (newRecordId <= 20) return RecordTypeEnum.Bounty;

                        return RecordTypeEnum.Stage;
                    })
                    .RuleFor(m => m.Notes, (f, m) =>
                    {
                        if (m.Type == RecordTypeEnum.Main) return f.PickRandom(mapDifficulty);

                        return f.Random.Int(1, 5).ToString();
                    })
                    .RuleFor(m => m.Time, f => f.Random.Float(100, 1000))
                    .RuleFor(m => m.CreateTime, f => DateTime.Now)
                    .RuleFor(m => m.UpDateTime, f => DateTime.Now)
                    .RuleFor(m => m.IsDelete, f => 0)
                    .Generate(30);
                context.Set<NewRecordModel>().AddRange(fakerNewRecordModel);
                //玩家通关
                var playerCompleteModelId = 1;
                var fakerPlayerCompleteModel = new Faker<PlayerCompleteModel>()
                    .RuleFor(m => m.Id, f => playerCompleteModelId++.ToString())
                    .RuleFor(m => m.PlayerId, f => f.Random.Int(0, 99).ToString())
                    .RuleFor(m => m.PlayerName, (f, m) => fakerPlayerModel.Where(t => t.Id == m.PlayerId).First().Name)
                    .RuleFor(m => m.MapId, f => f.Random.Int(1, 99).ToString())
                    .RuleFor(m => m.MapName, (f, m) => fakerMapModel.Where(t => t.Id == m.MapId).First().Name)
                    .RuleFor(m => m.Type, f => (RecordTypeEnum)f.Random.Int(0, 2))
                    .RuleFor(m => m.Stage, (f, m) => m.Type != RecordTypeEnum.Main ? f.Random.Int(1, 5) : null)
                    .RuleFor(m => m.Time, f => f.Random.Float(100, 1000))
                    .RuleFor(m => m.Date, f => f.Date.Past())
                    .RuleFor(m => m.CreateTime, f => DateTime.Now)
                    .RuleFor(m => m.UpDateTime, f => DateTime.Now)
                    .RuleFor(m => m.IsDelete, f => 0)
                    .Generate(2000);
                //单独为地图1每个生成100条数据
                var map1PlayerCompleteModelId = 1;
                var fakerMap1PlayerCompleteModel = new Faker<PlayerCompleteModel>()
                    .RuleFor(m => m.Id, f => "map1" + map1PlayerCompleteModelId++)
                    .RuleFor(m => m.PlayerId, f => f.Random.Int(0, 99).ToString())
                    .RuleFor(m => m.PlayerName, (f, m) => fakerPlayerModel.Where(t => t.Id == m.PlayerId).First().Name)
                    .RuleFor(m => m.MapId, f => "1")
                    .RuleFor(m => m.MapName, (f, m) => fakerMapModel.Where(t => t.Id == m.MapId).First().Name)
                    .RuleFor(m => m.Type, f =>
                    {
                        if (map1PlayerCompleteModelId <= 100) return RecordTypeEnum.Main;

                        if (map1PlayerCompleteModelId <= 200) return RecordTypeEnum.Bounty;

                        return RecordTypeEnum.Stage;
                    })
                    .RuleFor(m => m.Stage, (f, m) => m.Type != RecordTypeEnum.Main ? f.Random.Int(1, 5) : null)
                    .RuleFor(m => m.Time, f => f.Random.Float(100, 1000))
                    .RuleFor(m => m.Date, f => f.Date.Past())
                    .RuleFor(m => m.Hide, false)
                    .RuleFor(m => m.CreateTime, f => DateTime.Now)
                    .RuleFor(m => m.UpDateTime, f => DateTime.Now)
                    .RuleFor(m => m.IsDelete, f => 0)
                    .Generate(300);
                //去重
                var distinctList = fakerPlayerCompleteModel
                    .GroupBy(t => new
                    {
                        t.PlayerId,
                        t.MapId,
                        t.Type,
                        t.Stage
                    }).Select(t => t.First())
                    .ToList();
                distinctList.AddRange(fakerMap1PlayerCompleteModel);
                context.Set<PlayerCompleteModel>().AddRange(distinctList);
                context.SaveChanges();
            });
    }
}