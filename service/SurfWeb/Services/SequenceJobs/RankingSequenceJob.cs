using Common.Quartz;
using Quartz;
using Services.IServices;

namespace Services.SequenceJobs
{
    /// <summary>
    /// 排行
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
}
