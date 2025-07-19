using System.Reflection;
using Common.Quartz;
using Common.Quartz.SequenceJobs;
using Quartz;

namespace Configurations;

/// <summary>
/// </summary>
public static class JobConfiguration
{
    /// <summary>
    ///     配置数据库相关服务
    /// </summary>
    /// <param name="builder"></param>
    public static void AddJobConfiguration(this WebApplicationBuilder builder)
    {
        //注册定时任务
        builder.Services.Scan(scan => scan
            .FromAssemblies(Assembly.Load("Services"), Assembly.Load("Common"))
            .AddClasses(classes => classes.AssignableTo<ISequenceJob>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        //配置顺序执行的job
        builder.Services.AddScoped(provider =>
        {
            //job执行顺序
            var exeOrder = new List<Type>
            {
                ////玩家信息前
                //typeof(PlayerBeforeSequenceJob),
                ////地图信息
                //typeof(MapBeforeSequenceJob),
                ////玩家记录
                //typeof(PlayerCompleteSequenceJob),
                ////玩家信息后
                //typeof(PlayerLaterSequenceJob),
                ////地图信息后
                //typeof(MapLaterSequenceJob),
                ////排行
                //typeof(RankingSequenceJob),
                ////新记录
                //typeof(NewRecordSequenceJob),
                //缓存
                typeof(CacheSequenceJob)
            };
            return provider.GetServices<ISequenceJob>()
                .Where(t => exeOrder.Contains(t.GetType()))
                .OrderBy(t => exeOrder.IndexOf(t.GetType()))
                .ToList();
        });
        // 注册 Job（注册的Job必须添加Trigger否则会报错）
        builder.Services.AddQuartz(q =>
        {
            q.AddJob<SequenceJob>(opts => opts.WithIdentity("SequenceJob"));
            q.AddJob<ServerInfoCacheJob>(opts => opts.WithIdentity("ServerInfoCacheJob"));
            var jobConfig = builder.Configuration.GetSection("JobConfig").Get<JobConfig>();
            if (jobConfig == null) throw new InvalidOperationException("注册定时任务配置JobInterval部分缺失");
            q.AddTrigger(opts => opts
                    .ForJob("SequenceJob") // 关联到名为 "CacheJob" 的 Job
                    .WithIdentity("SequenceJob_Trigger") // 触发器的唯一标识名
                    .WithSimpleSchedule(x => x //简单调度计划
                        .WithIntervalInMinutes(jobConfig.SequenceJobMinute) // 每xx分钟执行一次
                        .RepeatForever()) // 永久重复执行
            );
            q.AddTrigger(opts => opts
                    .ForJob("ServerInfoCacheJob") 
                    .WithIdentity("ServerInfoCacheJob_Trigger")
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(jobConfig.ServerInfoCacheJobSecond) 
                        .RepeatForever())
            );
        });
        //添加Quartz到HostedService服务
        builder.Services.AddQuartzHostedService();
    }
}