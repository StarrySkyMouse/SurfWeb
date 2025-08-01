using Common.Logger.Sign;
using Common.Quartz.Base;
using Configurations.QuartzSetup.SequenceJobs.DbSync.Items;
using Microsoft.Extensions.Logging;

namespace Configurations.QuartzSetup.SequenceJobs.DbSync;

/// <summary>
///     数据同步
/// </summary>
public class DbSyncSequenceJob : BaseSequenceJob
{
    public DbSyncSequenceJob(ILogger<IConsoleLoggerSign> logger, IServiceProvider provider) : base(logger, provider)
    {
    }

    public override List<Type> GetSequenceJob()
    {
        return new List<Type>
        {
            //玩家信息前
            typeof(PlayerBeforeSequenceJob),
            //地图信息
            typeof(MapBeforeSequenceJob),
            //玩家记录
            typeof(PlayerCompleteSequenceJob),
            //玩家信息后
            typeof(PlayerLaterSequenceJob),
            //地图信息后
            typeof(MapLaterSequenceJob),
        };
    }
}