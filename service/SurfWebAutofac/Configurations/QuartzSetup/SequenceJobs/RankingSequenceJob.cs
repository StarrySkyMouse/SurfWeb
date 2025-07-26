using Common.Quartz.Base;
using IServices.Main;
using Quartz;

namespace Configurations.QuartzSetup.SequenceJobs;

/// <summary>
///     排行
/// </summary>
public class RankingSequenceJob : ISequenceJob
{
    private readonly IRankingServices _rankingServices;

    public RankingSequenceJob(
        IRankingServices rankingServices)
    {
        _rankingServices = rankingServices;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        //更新排行
        await _rankingServices.UpdateRanking();
    }
}