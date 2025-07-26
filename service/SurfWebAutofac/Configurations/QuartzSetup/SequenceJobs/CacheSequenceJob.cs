using Common.Quartz.Base;
using Quartz;

namespace Configurations.QuartzSetup.SequenceJobs;

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