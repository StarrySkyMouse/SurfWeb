using System.Reflection;
using Common.Quartz;
using Common.Quartz.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace Configurations;

public static class QuartzConfiguration
{
    /// <summary>
    ///     缓存
    /// </summary>
    /// <param name="builder"></param>
    public static void AddCacheConfiguration(this WebApplicationBuilder builder)
    {
        //注册定时任务
        // 注册 Job（注册的Job必须添加Trigger否则会报错）
        builder.Services.AddQuartz(q =>
        {
            // 加载相关程序集
            var sequenceJobProviders = Assembly
                .LoadFrom(Path.Combine(AppContext.BaseDirectory, "Services.dll")).GetTypes()
                .Where(t => typeof(SequenceJob).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
                .ToList();
            foreach (var item in sequenceJobProviders) q.AddJob(item, null, opts => opts.WithIdentity(item.FullName));

            var baseJobProviders = Assembly
                .LoadFrom(Path.Combine(AppContext.BaseDirectory, "Services.dll")).GetTypes()
                .Where(t => typeof(BaseJob).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
                .ToList();
            foreach (var item in baseJobProviders) q.AddJob(item, null, opts => opts.WithIdentity(item.FullName));

            var jobConfig = builder.Configuration.GetSection("JobConfig").Get<JobConfig>();
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
        builder.Services.AddQuartzHostedService();
    }
}