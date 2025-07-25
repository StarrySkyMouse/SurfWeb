using Common.Logger.Sign;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Common.Quartz.Base;

/// <summary>
///     顺序Job
/// </summary>
[DisallowConcurrentExecution] //防止重复触发
public abstract class SequenceJob : IJob
{
    private readonly ILogger<IConsoleLoggerSign> _logger;
    private readonly List<ISequenceJob> _sequenceJob;

    public SequenceJob(ILogger<IConsoleLoggerSign> logger)
    {
        _logger = logger;
        _sequenceJob = GetSequenceJob();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("定时任务执行时间: {time}", DateTimeOffset.Now);
            foreach (var item in _sequenceJob)
            {
                _logger.LogInformation($"同步任务{item.GetType().Name}执行时间: {DateTimeOffset.Now}");
                await item.Execute(context);
                _logger.LogInformation($"同步任务{item.GetType().Name}执行完成时间: {DateTimeOffset.Now}");
            }

            _logger.LogInformation("定时任务执行完成时间: {time}", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "定时任务异常");
        }
    }

    public abstract List<ISequenceJob> GetSequenceJob();
}