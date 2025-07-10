using Common.Quartz;
using Quartz;
using Services.Caches;

namespace Services.SequenceJobs
{
    /// <summary>
    /// 缓存更新
    /// </summary>
    public class CacheSequenceJob : ISequenceJob
    {
        private readonly DataCache _dataCache;
        public CacheSequenceJob(DataCache dataCache)
        {
            _dataCache = dataCache;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _dataCache.UpdateCache();
        }
    }
}