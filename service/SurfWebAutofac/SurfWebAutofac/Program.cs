using Configurations;
using Configurations.AutofacSetup;
using Configurations.SqlsugarSetup;

var builder = WebApplication.CreateBuilder(args);

//����Autofac
builder.AddAutofacConfiguration();
//����WebApi
builder.AddWebApiConfiguration();
//�������ݿ�
builder.AddSqlsugarConfiguration();

var app = builder.Build();

//WebApi�м��
app.UseAppMiddleware();

app.Run();