using Common.Quartz;
using Configurations.QuartzSetup.SequenceJobs.DbSync;
using Microsoft.AspNetCore.Builder;

namespace Configurations.QuartzSetup;

public static class QuartzConfiguration
{
    /// <summary>
    ///     缓存
    /// </summary>
    /// <param name="builder"></param>
    public static void AddQuartzConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddQuartzService(builder.Configuration,
            cfg => cfg.AddSequenceJob<DbSyncSequenceJob>());
    }
}