using Core.Dapper;
using Core.IServices;
using Quartz;

namespace Core.Utils.Jobs.SequenceJobs
{
    /// <summary>
    /// 地图同步后
    /// </summary>
    public class MapLaterSequenceJob : ISequenceJob
    {
        private readonly ISqlHelp _sqlHelp;
        private readonly IMapServices _mapServices;
        public MapLaterSequenceJob(
            ISqlHelp sqlHelp,
            IMapServices mapServices
            )
        {
            _sqlHelp = sqlHelp;
            _mapServices = mapServices;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //更新地图完成人数
            await _mapServices.UpdateSucceesNumber();
        }
    }
}
