using Configurations;

var builder = WebApplication.CreateBuilder(args);

//���������ļ�
builder.AddLoadConfiguration();
//����WebApi
builder.AddWebApiConfiguration();
//�����������ݿ���ط���
builder.AddDbConfiguration();
//ҵ��ģ��IOCע������
builder.AddBusinessIocConfiguration();
//�������ݿ���ط���
builder.AddJobConfiguration();
//���û���
builder.AddCacheConfiguration();

var app = builder.Build();

//WebApi�м��
app.UseAppMiddleware();

app.Run();