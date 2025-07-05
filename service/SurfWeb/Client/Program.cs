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
        options.JsonSerializerOptions.Converters.Add(new FloatConverter());
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
builder.Services.AddSingleton(sp =>
{
    var dbConfig = sp.GetRequiredService<IConfiguration>().GetSection("DataSourceConfig").Get<DataSourceConfig>();
    if (dbConfig == null)
    {
        throw new InvalidOperationException("���ݿ�����DataSourceConfig����ȱʧ");
    }
    return dbConfig;
});
builder.Services.AddSingleton<ISqlHelp, MySqlHelp>();
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
builder.Services.Scan(scan => scan
    .FromAssemblies(Assembly.Load("Core"))
    .AddClasses(classes => classes.AssignableTo<ISequenceJob>())
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);
//����˳��ִ�е�job
builder.Services.AddScoped(provider =>
{
    //jobִ��˳��
    var exeOrder = new List<Type>()
    {
        //�����Ϣǰ
        typeof(PlayerBeforeSequenceJob),
        //��ͼ��Ϣ
        typeof(MapBeforeSequenceJob),
        //��Ҽ�¼
        typeof(PlayerCompleteSequenceJob),
        //�����Ϣ��
        typeof(PlayerLaterSequenceJob),
        //��ͼ��Ϣ��
        typeof(MapLaterSequenceJob),
        //����
        typeof(RankingSequenceJob),
        //�¼�¼
        typeof(NewRecordSequenceJob),
        //����
        typeof(CacheSequenceJob),
    };
    return provider.GetServices<ISequenceJob>()
        .Where(t => exeOrder.Contains(t.GetType()))
        .OrderBy(t => exeOrder.IndexOf(t.GetType()))
        .ToList();
});
// ע�� Job��ע���Job�������Trigger����ᱨ��
builder.Services.AddQuartz(q =>
{
    q.AddJob<SequenceJob>(opts => opts.WithIdentity("SequenceJob"));
    var jobInterval = builder.Configuration.GetSection("JobInterval").Get<int?>();
    if (jobInterval == null)
    {
        throw new InvalidOperationException("ע�ᶨʱ��������JobInterval����ȱʧ");
    }
    q.AddTrigger(opts => opts
        .ForJob("SequenceJob")// ��������Ϊ "CacheJob" �� Job
        .WithIdentity("SequenceJob_Trigger")// ��������Ψһ��ʶ��
        .WithSimpleSchedule(x => x//�򵥵��ȼƻ�
            .WithIntervalInMinutes(jobInterval.Value)// ÿxx����ִ��һ��
            .RepeatForever())// �����ظ�ִ��
        );
});
//���Quartz��HostedService����
builder.Services.AddQuartzHostedService();
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
