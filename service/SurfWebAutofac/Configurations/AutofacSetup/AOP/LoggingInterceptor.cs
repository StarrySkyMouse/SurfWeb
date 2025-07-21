using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace Configurations.AutofacSetup.AOP;

public class LoggingInterceptor : IInterceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        var methodName = invocation.Method.Name;
        var typeName = invocation.TargetType.Name;
        var arguments = string.Join(", ", invocation.Arguments.Select(a => a?.ToString() ?? "null"));

        _logger.LogInformation($"Entering {typeName}.{methodName} with arguments [{arguments}]");
        try
        {
            invocation.Proceed();
            _logger.LogInformation($"Exiting {typeName}.{methodName} with result [{invocation.ReturnValue}]");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception in {typeName}.{methodName}");
            throw;
        }
    }
}