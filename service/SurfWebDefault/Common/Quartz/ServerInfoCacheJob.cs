using Common.Caches;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Common.Quartz
{
    /// <summary>
    /// 服务器信息缓存更新Job
    /// </summary>
    [DisallowConcurrentExecution] //防止重复触发
    public class ServerInfoCacheJob : IJob
    {
        private readonly ILogger<ServerInfoCacheJob> _logger;
        private readonly CacheManage _cacheManage;

        public ServerInfoCacheJob(ILogger<ServerInfoCacheJob> logger,
            CacheManage cacheManage)
        {
            _cacheManage = cacheManage;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("定时任务执行时间: {time}", DateTimeOffset.Now);
                await _cacheManage.UpdateServiceInfoCache();
                _logger.LogInformation("定时任务执行完成时间: {time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "定时任务异常");
            }
        }
    }
}
