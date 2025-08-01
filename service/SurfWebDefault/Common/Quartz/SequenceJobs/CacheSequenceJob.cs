using Common.Caches;
using Quartz;

namespace Common.Quartz.SequenceJobs;

/// <summary>
///     缓存更新
/// </summary>
public class CacheSequenceJob : ISequenceJob
{
    private readonly CacheManage _cacheManage;

    public CacheSequenceJob(CacheManage cacheManage)
    {
        _cacheManage = cacheManage;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _cacheManage.UpdateMapCache();
    }
}