using System.Reflection;
using DataSync.SourceModel;
using Microsoft.Extensions.Configuration;
using Model.Models.Base;
using MySqlConnector;
using Repository.Other;
using SqlSugar;

SqlSugarClient _db;
MySqlHelp _sourceDb;

SyncData();
//数据同步
void SyncData()
{
    Init();
    //初始化主库（只需要执行一次）
    InitDb();
    TransferData();
    Console.WriteLine("已完成按任意键结束");
    Console.ReadKey();
}
void Init()
{
    Console.WriteLine("对象初始化");
    //当本地没有appsettings.Development.json时，使用开源配置
    var env = "Development";
    if (!File.Exists("appsettings.Development.json"))
    {
        env = "OpenSource";
    }

    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory()) // Microsoft.Extensions.Configuration.FileExtensions 的扩展方法
        .AddJsonFile($"appsettings.{env}.json", optional: false,
            reloadOnChange: true) //Microsoft.Extensions.Configuration.Json 的扩展方法
        .Build();

    var dbConfig =
        config.GetSection("DbConnection").Get<string>() ??
        throw new Exception("无法获取DbConnection配置"); //Microsoft.Extensions.Configuration.Binder的扩展方法
    _db = new SqlSugarClient(new ConnectionConfig
    {
        ConnectionString = dbConfig,
        DbType = DbType.MySql,
        IsAutoCloseConnection = true,
    });
    var sourceDbConfig =
        config.GetSection("SourceDbConnection").Get<string>() ??
        throw new Exception("无法获取SourceDbConnection配置");
    _sourceDb = new MySqlHelp(sourceDbConfig);
    Console.WriteLine("完成对象初始化");
}
//初始化主库
void InitDb()
{
    Console.WriteLine("数据库初始化");
    _db.DbMaintenance.CreateDatabase();
    var baseType = typeof(BaseEntity);
    var list = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "Model.dll"))
        .GetTypes()
        .Where(t => t is { IsClass: true, IsAbstract: false } && baseType.IsAssignableFrom(t))
        .ToArray();
    //创建表
    _db.CodeFirst.InitTables(list);
    //删除所有数据
    _db.DbMaintenance.TruncateTable<Model.Models.Main.MapModel>();
    _db.DbMaintenance.TruncateTable<Model.Models.Main.NewRecordModel>();
    _db.DbMaintenance.TruncateTable<Model.Models.Main.PlayerModel>();
    _db.DbMaintenance.TruncateTable<Model.Models.Main.PlayerCompleteModel>();
    _db.DbMaintenance.TruncateTable<Model.Models.Main.RankingModel>();
    Console.WriteLine("完成数据库初始化");
}
//转换数据
void TransferData()
{
    Console.WriteLine("数据插入");
    //map
    var mapList = GetSourceMapList();
    _db.Insertable<Model.Models.Main.MapModel>(mapList.Select(t => new Model.Models.MapModel()
    {
        Name = t.Name,
        Difficulty = t.Difficulty,
        Img = t.Img,
        SurcessNumber = t.SurcessNumber,
        BonusNumber = t.BonusNumber,
        StageNumber = t.StageNumber,
        CreateTime = t.CreateTime,
        UpDateTime = t.UpDateTime,
        IsDelete = t.IsDelete
    }).ToList()).ExecuteReturnSnowflakeIdList();
    Console.WriteLine($"map:{mapList.Count}条");
    //newrecord
    var newrecordList = GetSourceNewrecordList();
    _db.Insertable<Model.Models.Main.NewRecordModel>(newrecordList.Select(t => new Model.Models.NewRecordModel()
    {
        MapId = 0,
        MapName = null,
        Type = (Model.Models.Main.RecordTypeEnum)(int)t.Type,
        Notes = t.Notes,
        Time = t.Time,
        Date = t.Date,
        CreateTime = t.CreateTime,
        UpDateTime = t.UpDateTime,
        IsDelete = t.IsDelete
    }).ToList()).ExecuteReturnSnowflakeIdList();
    Console.WriteLine($"newrecord:{newrecordList.Count}条");
    //player
    var playerList = GetSourcePlayerList();
    _db.Insertable<Model.Models.Main.PlayerModel>(playerList.Select(t => new Model.Models.PlayerModel()
    {
        Name = t.Name,
        Integral = t.Integral,
        SucceesNumber = t.SucceesNumber,
        WRNumber = t.WRNumber,
        BWRNumber = t.BWRNumber,
        SWRNumber = t.SWRNumber,
        CreateTime = t.CreateTime,
        UpDateTime = t.UpDateTime,
        IsDelete = t.IsDelete
    }).ToList()).ExecuteReturnSnowflakeIdList();
    Console.WriteLine($"player:{playerList.Count}条");
    //playercomplete
    var playerCompleteList = GetSourcePlayercompleteList();
    _db.Insertable<Model.Models.Main.PlayerCompleteModel>(playerCompleteList.Select(t =>
        new Model.Models.PlayerCompleteModel()
        {
            Auth = t.Auth,
            PlayerId = 0,
            PlayerName = null,
            MapId = 0,
            MapName = null,
            Type = (Model.Models.Main.RecordTypeEnum)(int)t.Type,
            Stage = t.Stage,
            Time = t.Time,
            Date = t.Date,
            Hide = t.Hide,
            CreateTime = t.CreateTime,
            UpDateTime = t.UpDateTime,
            IsDelete = t.IsDelete
        }).ToList()).ExecuteReturnSnowflakeIdList();
    Console.WriteLine($"playercomplete:{playerCompleteList.Count}条");
    //ranking
    var rankingList = GetSourceRankingList();
    _db.Insertable<Model.Models.Main.RankingModel>(rankingList.Select(t => new Model.Models.RankingModel()
    {
        Type = (Model.Models.Main.RankingTypeEnum)(int)t.Type,
        Rank = t.Rank,
        PlayerId = 0,
        PlayerName = null,
        Value = t.Value,
        CreateTime = t.CreateTime,
        UpDateTime = t.UpDateTime,
        IsDelete = t.IsDelete
    }).ToList()).ExecuteReturnSnowflakeIdList();
    Console.WriteLine($"ranking:{rankingList.Count}条");
}

List<DataSync.SourceModel.MapModel> GetSourceMapList()
{
    int pageIndex = 1;
    int pageSize = 1000;
    var result = new List<DataSync.SourceModel.MapModel>();
    while (true)
    {
        var list = _sourceDb.QueryPageAsync<DataSync.SourceModel.MapModel>("select * from map", pageIndex, pageSize)
            .Result;
        if (list == null || !list.Any())
            break;
        result.AddRange(list);
        pageIndex++;
    }

    return result;
}
List<DataSync.SourceModel.NewRecordModel> GetSourceNewrecordList()
{
    int pageIndex = 1;
    int pageSize = 1000;
    var result = new List<DataSync.SourceModel.NewRecordModel>();
    while (true)
    {
        var list = _sourceDb.QueryPageAsync<DataSync.SourceModel.NewRecordModel>("select * from newrecord", pageIndex, pageSize)
            .Result;
        if (list == null || !list.Any())
            break;
        result.AddRange(list);
        pageIndex++;
    }

    return result;
}

List<DataSync.SourceModel.PlayerModel> GetSourcePlayerList()
{
    int pageIndex = 1;
    int pageSize = 1000;
    var result = new List<DataSync.SourceModel.PlayerModel>();
    while (true)
    {
        var list = _sourceDb.QueryPageAsync<DataSync.SourceModel.PlayerModel>("select * from player", pageIndex, pageSize)
            .Result;
        if (list == null || !list.Any())
            break;
        result.AddRange(list);
        pageIndex++;
    }

    return result;
}

List<DataSync.SourceModel.PlayerCompleteModel> GetSourcePlayercompleteList()
{
    int pageIndex = 1;
    int pageSize = 1000;
    var result = new List<DataSync.SourceModel.PlayerCompleteModel>();
    while (true)
    {
        var list = _sourceDb.QueryPageAsync<DataSync.SourceModel.PlayerCompleteModel>("select * from playercomplete", pageIndex, pageSize)
            .Result;
        if (list == null || !list.Any())
            break;
        result.AddRange(list);
        pageIndex++;
    }

    return result;
}

List<DataSync.SourceModel.RankingModel> GetSourceRankingList()
{
    int pageIndex = 1;
    int pageSize = 1000;
    var result = new List<DataSync.SourceModel.RankingModel>();
    while (true)
    {
        var list = _sourceDb.QueryPageAsync<DataSync.SourceModel.RankingModel>("select * from ranking", pageIndex, pageSize)
            .Result;
        if (list == null || !list.Any())
            break;
        result.AddRange(list);
        pageIndex++;
    }

    return result;
}