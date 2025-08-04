using System.Diagnostics;
using System.Reflection;
using Castle.DynamicProxy;
using Common.Logger.Dto;
using Common.Logger.Sign;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Logger.AOP;

public class ServiceLoggingInterceptor : IInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<IServiceLoggerSign> _logger;

    public ServiceLoggingInterceptor(ILogger<IServiceLoggerSign> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Intercept(IInvocation invocation)
    {
        var methodName = invocation.Method.Name;
        var typeName = invocation.TargetType.Name;
        var arguments = string.Join(", ", invocation.Arguments.Select(a => a?.ToString() ?? "null"));
        var stopwatch = Stopwatch.StartNew();

        invocation.Proceed();
        if (invocation.Method.ReturnType == typeof(Task))
        {
            invocation.ReturnValue = InterceptAsync((Task)invocation.ReturnValue, methodName, arguments, stopwatch);
        }
        else if (invocation.Method.ReturnType.IsGenericType &&
                 invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
            var method = typeof(ServiceLoggingInterceptor).GetMethod(nameof(InterceptAsyncGeneric),
                BindingFlags.NonPublic | BindingFlags.Instance);
            var genericMethod = method.MakeGenericMethod(resultType);
            invocation.ReturnValue = genericMethod.Invoke(this,
                new[] { invocation.ReturnValue, methodName, arguments, stopwatch });
        }
        else
        {
            // 同步
            stopwatch.Stop();
            _logger.LogInformation(ServiceLoggerDto.InfoToString(_httpContextAccessor.HttpContext.TraceIdentifier,
                ServiceLoggerDtoType.Info, methodName,
                arguments, invocation.ReturnValue, stopwatch.ElapsedMilliseconds));
        }
    }

    private async Task InterceptAsync(Task task, string methodName, string arguments, Stopwatch stopwatch)
    {
        try
        {
            await task;
            stopwatch.Stop();
            _logger.LogInformation(ServiceLoggerDto.InfoToString(_httpContextAccessor.HttpContext.TraceIdentifier,
                ServiceLoggerDtoType.Info, methodName,
                arguments, null, stopwatch.ElapsedMilliseconds));
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ServiceLoggerDto.ErrorToString(_httpContextAccessor.HttpContext.TraceIdentifier,
                ServiceLoggerDtoType.Error, methodName, arguments,
                null, stopwatch.ElapsedMilliseconds, ex.ToString()));
            throw;
        }
    }

    private async Task<T> InterceptAsyncGeneric<T>(Task<T> task, string methodName, string arguments,
        Stopwatch stopwatch)
    {
        try
        {
            var result = await task;
            stopwatch.Stop();
            _logger.LogInformation(ServiceLoggerDto.InfoToString(_httpContextAccessor.HttpContext.TraceIdentifier,
                ServiceLoggerDtoType.Info, methodName,
                arguments, result, stopwatch.ElapsedMilliseconds));
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ServiceLoggerDto.ErrorToString(_httpContextAccessor.HttpContext.TraceIdentifier,
                ServiceLoggerDtoType.Error, methodName, arguments,
                null, stopwatch.ElapsedMilliseconds, ex.ToString()));
            throw;
        }
    }
}