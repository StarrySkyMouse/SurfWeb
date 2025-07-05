using Core;
using Core.Dapper;
using Core.Utils.Formattings;
using Core.Utils.GlobalParams;
using Core.Utils.Jobs;
using Core.Utils.Jobs.SequenceJobs;
using Core.Utils.Middlewares;
using Quartz;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#region 分离本地配置和开源配置
// 先清空默认配置源
builder.Configuration.Sources.Clear();
// 添加主配置文件
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// 获取环境变量
var env = builder.Environment.EnvironmentName;
// 判断 Development 配置文件是否存在
var devConfigFile = $"appsettings.{env}.json";
var openSourceConfigFile = "appsettings.OpenSource.json";
if (env == "Development" && !File.Exists(devConfigFile) && File.Exists(openSourceConfigFile))
{
    // 如果是开发环境且没有 appsettings.Development.json，但有 OpenSource 配置，则加载 OpenSource
    builder.Configuration.AddJsonFile(openSourceConfigFile, optional: false, reloadOnChange: true);
}
else
{
    // 正常加载当前环境配置文件
    builder.Configuration.AddJsonFile(devConfigFile, optional: true, reloadOnChange: true);
}
builder.Configuration.AddEnvironmentVariables();
#endregion

// 注册控制器和数据格式化
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateTiemConverter());
        options.JsonSerializerOptions.Converters.Add(new FloatConverter());
        options.JsonSerializerOptions.Converters.Add(new DecimalConverter());
    });
// 注册Swagger
builder.Services.AddEndpointsApiExplorer();
//当swagger无法使用时请尝试清除浏览器缓存！！！！！！！！！！！！
builder.Services.AddSwaggerGen(options =>
{
    //读取xml注释文件(勾选生成api文档)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, true);
});
builder.Services.AddSingleton(sp =>
{
    var dbConfig = sp.GetRequiredService<IConfiguration>().GetSection("DbConfig").Get<DbConfig>();
    if (dbConfig == null)
    {
        throw new InvalidOperationException("数据库配置DbConfig部分缺失");
    }
    return dbConfig;
});
builder.Services.AddSingleton(sp =>
{
    var dbConfig = sp.GetRequiredService<IConfiguration>().GetSection("DataSourceConfig").Get<DataSourceConfig>();
    if (dbConfig == null)
    {
        throw new InvalidOperationException("数据库配置DataSourceConfig部分缺失");
    }
    return dbConfig;
});
builder.Services.AddSingleton<ISqlHelp, MySqlHelp>();
// 注册数据库配置和EFCore
builder.Services.AddDbContext<SurfWebDbContext>();

//通过扫描的方式注册
builder.Services.Scan(scan => scan
    //加载名为 "Core" 的程序集（DLL）    
    .FromAssemblies(Assembly.Load("Core"))
    //选择类名以 "Repository" 或 "Services" 结尾的类型
    .AddClasses(classes => classes.Where(type =>
        type.Name.EndsWith("Repository") || type.Name.EndsWith("Services")))
    //把这些类注册为它们实现的接口
    .AsImplementedInterfaces()
    //注册为作用域生命周期（Scoped）
    .WithScopedLifetime()
);
//注册缓存
builder.Services.AddSingleton<DataCache>();
//注册CORS策略 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
//注册定时任务
builder.Services.Scan(scan => scan
    .FromAssemblies(Assembly.Load("Core"))
    .AddClasses(classes => classes.AssignableTo<ISequenceJob>())
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);
//配置顺序执行的job
builder.Services.AddScoped(provider =>
{
    //job执行顺序
    var exeOrder = new List<Type>()
    {
        //玩家信息前
        typeof(PlayerBeforeSequenceJob),
        //地图信息
        typeof(MapBeforeSequenceJob),
        //玩家记录
        typeof(PlayerCompleteSequenceJob),
        //玩家信息后
        typeof(PlayerLaterSequenceJob),
        //地图信息后
        typeof(MapLaterSequenceJob),
        //排行
        typeof(RankingSequenceJob),
        //新记录
        typeof(NewRecordSequenceJob),
        //缓存
        typeof(CacheSequenceJob),
    };
    return provider.GetServices<ISequenceJob>()
        .Where(t => exeOrder.Contains(t.GetType()))
        .OrderBy(t => exeOrder.IndexOf(t.GetType()))
        .ToList();
});
// 注册 Job（注册的Job必须添加Trigger否则会报错）
builder.Services.AddQuartz(q =>
{
    q.AddJob<SequenceJob>(opts => opts.WithIdentity("SequenceJob"));
    var jobInterval = builder.Configuration.GetSection("JobInterval").Get<int?>();
    if (jobInterval == null)
    {
        throw new InvalidOperationException("注册定时任务配置JobInterval部分缺失");
    }
    q.AddTrigger(opts => opts
        .ForJob("SequenceJob")// 关联到名为 "CacheJob" 的 Job
        .WithIdentity("SequenceJob_Trigger")// 触发器的唯一标识名
        .WithSimpleSchedule(x => x//简单调度计划
            .WithIntervalInMinutes(jobInterval.Value)// 每xx分钟执行一次
            .RepeatForever())// 永久重复执行
        );
});
//添加Quartz到HostedService服务
builder.Services.AddQuartzHostedService();
var app = builder.Build();

// 调试环境下启用Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
//api响应中间件
app.UseMiddleware<ApiResponseMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
