using Core.Utils.GlobalParams;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils.HostedServices
{
    /// <summary>
    /// 定时缓存任务
    /// </summary>
    public class CacheTaskService : BackgroundService
    {
        private readonly ILogger<CacheTaskService> _logger;
        private readonly DataCache _dataCache;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5); // 5分钟执行一次

        public CacheTaskService(ILogger<CacheTaskService> logger, DataCache dataCache)
        {
            _logger = logger;
            _dataCache = dataCache;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 你的定时任务逻辑
                    _logger.LogInformation("定时任务执行时间: {time}", DateTimeOffset.Now);
                    await _dataCache.UpdateCache();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "定时任务异常");
                }
                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
