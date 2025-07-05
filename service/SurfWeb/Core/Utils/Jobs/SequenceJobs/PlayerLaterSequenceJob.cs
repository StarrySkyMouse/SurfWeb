using Core.IServices;
using Quartz;

namespace Core.Utils.Jobs.SequenceJobs
{
    /// <summary>
    /// 用户数据同步后
    /// </summary>
    public class PlayerLaterSequenceJob : ISequenceJob
    {
        private readonly IPlayerServices _playerServices;
        public PlayerLaterSequenceJob(
            IPlayerServices playerServices)
        {
            _playerServices = playerServices;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //更新玩家信息
            await _playerServices.UpdateStatsNumber();
        }
    }
}
