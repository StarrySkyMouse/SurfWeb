using Castle.DynamicProxy;
using Common.Logger.Sign;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Common.Logger.AOP.ServiceLogging;

public class ServiceLoggingInterceptor : IInterceptor
{
    private readonly ILogger<IServiceLoggerSign> _logger;

    public ServiceLoggingInterceptor(ILogger<IServiceLoggerSign> logger)
    {
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        var methodName = invocation.Method.Name;
        var typeName = invocation.TargetType.Name;
        var arguments = string.Join(", ", invocation.Arguments.Select(a => a?.ToString() ?? "null"));
        var stopwatch = Stopwatch.StartNew();
        try
        {
            invocation.Proceed();
            stopwatch.Stop();
            _logger.LogInformation(
                $"调用接口:[{typeName}.{methodName}]|参数:[{arguments}]|返回结果:[{invocation.ReturnValue}]|用时:[{stopwatch.ElapsedMilliseconds}] ms");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(
                $"接口异常调用接口:[{typeName}.{methodName}]|参数:[{arguments}]|用时:[{stopwatch.ElapsedMilliseconds}] ms");
            throw;
        }
    }
}