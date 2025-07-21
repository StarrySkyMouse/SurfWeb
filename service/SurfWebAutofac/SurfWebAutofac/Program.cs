using Configurations;
using Configurations.AutofacSetup;
using Configurations.SqlsugarSetup;

var builder = WebApplication.CreateBuilder(args);

//配置Autofac
builder.AddAutofacConfiguration();
//配置WebApi
builder.AddWebApiConfiguration();
//配置数据库
builder.AddSqlsugarConfiguration();

var app = builder.Build();

//WebApi中间件
app.UseAppMiddleware();

app.Run();