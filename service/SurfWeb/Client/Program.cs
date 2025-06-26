using Core;
using System.Reflection;
using Core.Utils.Middlewares;
using Core.Utils.Formattings;
using Core.Utils.GlobalParams;
using Core.Utils.HostedServices;

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
        options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
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
builder.Services.AddHostedService<CacheTaskService>();
builder.Services.AddHostedService<DataTaskService>();

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
