using Common.Quartz.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Common.Quartz;

public static class ServiceCollectionExtensions
{
    public static void AddQuartzService(this IServiceCollection services, IConfiguration configuration,
        Action<IQuartzConfigure> actionCfg)
    {
        var quartzConfigure = new QuartzConfigure();
        actionCfg(quartzConfigure);
        ////顺序任务记录
        foreach (var item in quartzConfigure.GetBaseSequenceJobs()) services.AddScoped(typeof(BaseSequenceJob), item);

        foreach (var item in quartzConfigure.GetSequenceJobs()) services.AddScoped(item);

        foreach (var item in quartzConfigure.GetJobs()) services.AddScoped(typeof(BaseJob), item);

        services.AddQuartz(q =>
        {
            //注册定时任务
            // 注册 Job（注册的Job必须添加Trigger否则会报错）
            foreach (var item in quartzConfigure.GetBaseSequenceJobs())
                q.AddJob(item, null, opts => opts.WithIdentity(item.Name));
            foreach (var item in quartzConfigure.GetJobs())
                q.AddJob(item, null, opts => opts.WithIdentity(item.Name));
            var jobConfig = configuration.GetSection("JobConfig").Get<JobConfig>();
            if (jobConfig == null) throw new InvalidOperationException("注册定时任务配置JobConfig部分缺失");
            //配置同步任务执行
            jobConfig.SequenceJobConfigs.ForEach(t =>
            {
                q.AddTrigger(opts => opts
                        .ForJob(t.Name) // 关联到名为 "CacheJob" 的 Job
                        .WithIdentity($"{t.Name}_Trigger") // 触发器的唯一标识名
                        .WithSimpleSchedule(x => x //简单调度计划
                            .WithIntervalInMinutes(t.SecondMinute) // 每xx分钟执行一次
                            .RepeatForever()) // 永久重复执行
                );
            });
            //配置普通异步任务
            jobConfig.JobConfigs.ForEach(t =>
            {
                q.AddTrigger(opts => opts
                        .ForJob(t.Name) // 关联到名为 "CacheJob" 的 Job
                        .WithIdentity($"{t.Name}_Trigger") // 触发器的唯一标识名
                        .WithSimpleSchedule(x => x //简单调度计划
                            .WithIntervalInMinutes(t.SecondMinute) // 每xx分钟执行一次
                            .RepeatForever()) // 永久重复执行
                );
            });
        });
        //添加Quartz到HostedService服务
        services.AddQuartzHostedService();
    }

    public static IQuartzConfigure AddSequenceJob<T>(this IQuartzConfigure cfg)
        where T : BaseSequenceJob
    {
        cfg.GetBaseSequenceJobs().Add(typeof(T));
        // 此处仅用于获取 T 下属的 SequenceJob 类型，不做实际构造
        var temp = Activator.CreateInstance(typeof(T), new object[] { null, null }) as BaseSequenceJob;
        if (temp != null) cfg.GetSequenceJobs().AddRange(temp.GetSequenceJob());
        return cfg;
    }

    public static IQuartzConfigure AddJob<T>(this IQuartzConfigure cfg)
        where T : BaseJob
    {
        cfg.GetJobs().Add(typeof(T));
        return cfg;
    }
}

public interface IQuartzConfigure
{
    List<Type> GetBaseSequenceJobs();
    List<Type> GetSequenceJobs();
    List<Type> GetJobs();
}

public class QuartzConfigure : IQuartzConfigure
{
    private readonly List<Type> _baseSequenceJobs = new();
    private readonly List<Type> _jobs = new();
    private readonly List<Type> _sequenceJobs = new();

    public List<Type> GetBaseSequenceJobs()
    {
        return _baseSequenceJobs;
    }

    public List<Type> GetSequenceJobs()
    {
        return _sequenceJobs;
    }

    public List<Type> GetJobs()
    {
        return _jobs;
    }
}