using System.Reflection;
using Common.Db.Base;
using Common.Db.Dapper;
using DataSync.SourceModel;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using MapModel = Model.Models.Main.MapModel;
using PlayerCompleteModel = Model.Models.Main.PlayerCompleteModel;
using PlayerModel = Model.Models.Main.PlayerModel;
using RecordTypeEnum = Model.Models.Main.RecordTypeEnum;

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
    if (!File.Exists("appsettings.Development.json")) env = "OpenSource";

    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory()) // Microsoft.Extensions.Configuration.FileExtensions 的扩展方法
        .AddJsonFile($"appsettings.{env}.json", false,
            true) //Microsoft.Extensions.Configuration.Json 的扩展方法
        .Build();

    var dbConfig =
        config.GetSection("DbConnection").Get<string>() ??
        throw new Exception("无法获取DbConnection配置"); //Microsoft.Extensions.Configuration.Binder的扩展方法
    _db = new SqlSugarClient(new ConnectionConfig
    {
        ConnectionString = dbConfig,
        DbType = DbType.MySql,
        IsAutoCloseConnection = true
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
    _db.DbMaintenance.TruncateTable<MapModel>();
    _db.DbMaintenance.TruncateTable<PlayerModel>();
    _db.DbMaintenance.TruncateTable<PlayerCompleteModel>();
    Console.WriteLine("完成数据库初始化");
}

//转换数据
void TransferData()
{
    Console.WriteLine("数据插入");
    //map
    var mapList = GetSourceMapList();
    _db.Insertable(mapList.Select(t => new MapModel
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
    //player
    var playerList = GetSourcePlayerList();
    _db.Insertable(playerList.Select(t => new PlayerModel
    {
        Auth = t.Auth,
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
    var playerModelList = _db.Queryable<PlayerModel>().ToList();
    var mapModelList = _db.Queryable<MapModel>().ToList();
    var playerCompleteList = GetSourcePlayercompleteList();
    var insertList = playerCompleteList.Select(t =>
        new PlayerCompleteModel
        {
            Auth = t.Auth,
            PlayerId = playerModelList.Any(a => a.Auth == t.Auth)
                ? playerModelList.FirstOrDefault(a => a.Auth == t.Auth).Id
                : 0,
            PlayerName = t.PlayerName,
            MapId = mapModelList.Any(a => a.Name == t.MapName)
                ? mapModelList.FirstOrDefault(a => a.Name == t.MapName).Id
                : 0,
            MapName = t.MapName,
            Type = (RecordTypeEnum)(int)t.Type,
            Stage = t.Stage,
            Time = t.Time,
            Date = t.Date,
            Hide = t.Hide,
            CreateTime = t.CreateTime,
            UpDateTime = t.UpDateTime,
            IsDelete = t.IsDelete
        }).ToList();
    _db.Insertable(insertList).ExecuteReturnSnowflakeIdList();
    Console.WriteLine($"playercomplete:{playerCompleteList.Count}条");
}

List<DataSync.SourceModel.MapModel> GetSourceMapList()
{
    var pageIndex = 1;
    var pageSize = 1000;
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

List<DataSync.SourceModel.PlayerModel> GetSourcePlayerList()
{
    var pageIndex = 1;
    var pageSize = 1000;
    var result = new List<DataSync.SourceModel.PlayerModel>();
    while (true)
    {
        var list = _sourceDb
            .QueryPageAsync<DataSync.SourceModel.PlayerModel>("select * from player", pageIndex, pageSize)
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
    var pageIndex = 1;
    var pageSize = 1000;
    var result = new List<DataSync.SourceModel.PlayerCompleteModel>();
    while (true)
    {
        var list = _sourceDb
            .QueryPageAsync<DataSync.SourceModel.PlayerCompleteModel>("select * from playercomplete", pageIndex,
                pageSize)
            .Result;
        if (list == null || !list.Any())
            break;
        result.AddRange(list);
        pageIndex++;
    }

    return result;
}
