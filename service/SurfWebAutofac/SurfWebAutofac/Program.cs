using Configurations;
using Configurations.AutofacSetup;
using Configurations.AutoMapperSetup;
using Configurations.LoggerSetup;
using Configurations.SqlsugarSetup;

var builder = WebApplication.CreateBuilder(args);

//����Autofac
builder.AddAutofacConfiguration();
//����WebApi
builder.AddWebApiConfiguration();
//�������ݿ�
builder.AddSqlsugarConfiguration();
//���û���
builder.AddCacheConfiguration();
//������־
builder.AddLoggerConfiguration();
//��������
builder.AddAspNetCoreRateLimitConfiguration();
//����AutoMapper
builder.AddAutoMapperConfiguration();
//��ʱ����
builder.AddQuartzConfiguration();

var app = builder.Build();

//WebApi�м��
app.UseAppMiddleware();
//�����м��
app.UseAspNetCoreRateLimitMiddleware();

app.Run();