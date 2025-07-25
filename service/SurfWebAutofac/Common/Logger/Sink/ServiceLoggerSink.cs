using Serilog.Core;
using Serilog.Events;

namespace Common.Logger.Sink;

/// <summary>
///     服务端接口日志
/// </summary>
public class ServiceLoggerSink : ILogEventSink
{
    /// <summary>
    ///     属性注入
    /// </summary>
    public IServiceLoggerSinkExecute Execute { get; set; }

    public void Emit(LogEvent logEvent)
    {
        Execute.Emit(logEvent);
    }
}

/// <summary>
///     对外提供依赖注入的实现
/// </summary>
public interface IServiceLoggerSinkExecute
{
    void Emit(LogEvent logEvent);
}