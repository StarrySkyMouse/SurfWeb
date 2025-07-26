using Common.Logger.Sign;
using Common.Quartz.Base;
using Microsoft.Extensions.Logging;

namespace Configurations.QuartzSetup.SequenceJobs;

public class SequenceJob : BaseSequenceJob
{
    public SequenceJob(ILogger<IConsoleLoggerSign> logger, IServiceProvider provider) : base(logger, provider)
    {
    }

    public override List<Type> GetSequenceJob()
    {
        return new List<Type>
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
    }
}