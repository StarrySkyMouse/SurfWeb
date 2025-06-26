using Core;
using System.Reflection;
using Core.Utils.Middlewares;
using Core.Utils.Formattings;
using Core.Utils.GlobalParams;
using Core.Utils.HostedServices;

var builder = WebApplication.CreateBuilder(args);

#region ���뱾�����úͿ�Դ����
// �����Ĭ������Դ
builder.Configuration.Sources.Clear();
// ����������ļ�
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// ��ȡ��������
var env = builder.Environment.EnvironmentName;
// �ж� Development �����ļ��Ƿ����
var devConfigFile = $"appsettings.{env}.json";
var openSourceConfigFile = "appsettings.OpenSource.json";
if (env == "Development" && !File.Exists(devConfigFile) && File.Exists(openSourceConfigFile))
{
    // ����ǿ���������û�� appsettings.Development.json������ OpenSource ���ã������ OpenSource
    builder.Configuration.AddJsonFile(openSourceConfigFile, optional: false, reloadOnChange: true);
}
else
{
    // �������ص�ǰ���������ļ�
    builder.Configuration.AddJsonFile(devConfigFile, optional: true, reloadOnChange: true);
}
builder.Configuration.AddEnvironmentVariables();
#endregion

// ע������������ݸ�ʽ��
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateTiemConverter());
        options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
        options.JsonSerializerOptions.Converters.Add(new DecimalConverter());
    });
// ע��Swagger
builder.Services.AddEndpointsApiExplorer();
//��swagger�޷�ʹ��ʱ�볢�������������棡����������������������
builder.Services.AddSwaggerGen(options =>
{
    //��ȡxmlע���ļ�(��ѡ����api�ĵ�)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, true);
});
builder.Services.AddSingleton(sp =>
{
    var dbConfig = sp.GetRequiredService<IConfiguration>().GetSection("DbConfig").Get<DbConfig>();
    if (dbConfig == null)
    {
        throw new InvalidOperationException("���ݿ�����DbConfig����ȱʧ");
    }
    return dbConfig;
});
// ע�����ݿ����ú�EFCore
builder.Services.AddDbContext<SurfWebDbContext>();

//ͨ��ɨ��ķ�ʽע��
builder.Services.Scan(scan => scan
    //������Ϊ "Core" �ĳ��򼯣�DLL��    
    .FromAssemblies(Assembly.Load("Core"))
    //ѡ�������� "Repository" �� "Services" ��β������
    .AddClasses(classes => classes.Where(type =>
        type.Name.EndsWith("Repository") || type.Name.EndsWith("Services")))
    //����Щ��ע��Ϊ����ʵ�ֵĽӿ�
    .AsImplementedInterfaces()
    //ע��Ϊ�������������ڣ�Scoped��
    .WithScopedLifetime()
);
//ע�Ỻ��
builder.Services.AddSingleton<DataCache>();
//ע��CORS���� 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
//ע�ᶨʱ����
builder.Services.AddHostedService<CacheTaskService>();
builder.Services.AddHostedService<DataTaskService>();

var app = builder.Build();

// ���Ի���������Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
//api��Ӧ�м��
app.UseMiddleware<ApiResponseMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
