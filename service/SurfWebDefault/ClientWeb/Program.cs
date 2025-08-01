using Configurations;

var builder = WebApplication.CreateBuilder(args);

//加载配置文件
builder.AddLoadConfiguration();
//配置WebApi
builder.AddWebApiConfiguration();
//配置配置数据库相关服务
builder.AddDbConfiguration();
//业务模块IOC注册配置
builder.AddBusinessIocConfiguration();
//配置数据库相关服务
builder.AddJobConfiguration();
//配置缓存
builder.AddCacheConfiguration();

var app = builder.Build();

//WebApi中间件
app.UseAppMiddleware();

app.Run();