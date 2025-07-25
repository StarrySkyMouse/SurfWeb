using Configurations;
using Configurations.AutofacSetup;
using Configurations.AutoMapperSetup;
using Configurations.LoggerSetup;
using Configurations.SqlsugarSetup;

var builder = WebApplication.CreateBuilder(args);

//配置Autofac
builder.AddAutofacConfiguration();
//配置WebApi
builder.AddWebApiConfiguration();
//配置数据库
builder.AddSqlsugarConfiguration();
//配置缓存
builder.AddCacheConfiguration();
//配置日志
builder.AddLoggerConfiguration();
//配置限流
builder.AddAspNetCoreRateLimitConfiguration();
//配置AutoMapper
builder.AddAutoMapperConfiguration();
//定时任务
builder.AddQuartzConfiguration();

var app = builder.Build();

//WebApi中间件
app.UseAppMiddleware();
//限流中间件
app.UseAspNetCoreRateLimitMiddleware();

app.Run();