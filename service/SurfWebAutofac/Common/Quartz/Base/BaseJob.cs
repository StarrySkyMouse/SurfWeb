using Common.Logger.Sign;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Common.Quartz.Base;

public abstract class BaseJob : IJob
{
    private readonly ILogger<IConsoleLoggerSign> _logger;

    public BaseJob(ILogger<IConsoleLoggerSign> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation("定时任务执行时间: {time}", DateTimeOffset.Now);
            await MyExecute(context);
            _logger.LogInformation("定时任务执行完成时间: {time}", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "定时任务异常");
        }
    }

    public abstract Task MyExecute(IJobExecutionContext context);
}