using Bogus;
using Common.Db.Dapper;
using Common.Db.EFCore;
using Common.Db.SqlSugar;
using Microsoft.AspNetCore.Builder;
using Model.Models.Main;

namespace Configurations;

/// <summary>
///     数据库配置
/// </summary>
public static class DbConfiguration
{
    public static void AddSqlSugarConfiguration(this WebApplicationBuilder builder)
    {
        //添加SqlSugar服务
        builder.Services.AddSqlSugarService(builder.Configuration);
    }

    public static void AddDapperConfiguration(this WebApplicationBuilder builder)
    {
        //外部其他数据库
        builder.Services.AddDapperService(builder.Configuration);
    }

    public static void AddEFCoreConfiguration(this WebApplicationBuilder builder)
    {
        //添加EFCore服务
        builder.Services.AddEFCoreService(
            optionsBuilder =>
                optionsBuilder.UseSeeding((context, _) =>
                {
                    //清除所有数据
                    context.Set<MapModel>().RemoveRange(context.Set<MapModel>());
                    context.Set<PlayerCompleteModel>().RemoveRange(context.Set<PlayerCompleteModel>());
                    context.Set<PlayerModel>().RemoveRange(context.Set<PlayerModel>());
                    context.SaveChanges();
                    //地图
                    var mapDifficulty = new List<string> { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T0" };
                    var fakerMapModel = new Faker<MapModel>()
                        //.RuleFor(m => m.Id, f => mapId++)
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
                    context.SaveChanges();
                    //玩家
                    var fakerPlayerModel = new Faker<PlayerModel>()
                        //.RuleFor(m => m.Id, f => playerId++)
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
                    context.SaveChanges();
                    //玩家通关
                    var fakerPlayerCompleteModel = new Faker<PlayerCompleteModel>()
                        //.RuleFor(m => m.Id, f => playerCompleteModelId++)
                        .RuleFor(m => m.PlayerId, f => f.Random.Int(0, 99))
                        .RuleFor(m => m.PlayerName,
                            (f, m) => fakerPlayerModel.Where(t => t.Id == m.PlayerId).FirstOrDefault()?.Name)
                        .RuleFor(m => m.MapId, f => f.Random.Int(1, 99))
                        .RuleFor(m => m.MapName, (f, m) => fakerMapModel.Where(t => t.Id == m.MapId).FirstOrDefault()?.Name)
                        .RuleFor(m => m.Type, f => (RecordTypeEnum)f.Random.Int(0, 2))
                        .RuleFor(m => m.Stage, (f, m) => m.Type != RecordTypeEnum.Main ? f.Random.Int(1, 5) : null)
                        .RuleFor(m => m.Time, f => f.Random.Float(100, 1000))
                        .RuleFor(m => m.Date, f => f.Date.Past())
                        .RuleFor(m => m.CreateTime, f => DateTime.Now)
                        .RuleFor(m => m.UpDateTime, f => DateTime.Now)
                        .RuleFor(m => m.IsDelete, f => 0)
                        .Generate(2000);
                    context.Set<PlayerCompleteModel>().AddRange(fakerPlayerCompleteModel);
                    context.SaveChanges();
                }),
            modelBuilder =>
            {
                modelBuilder
                    .Entity<PlayerCompleteModel>()
                    .HasQueryFilter(t => t.Hide == false);
            });
    }
}