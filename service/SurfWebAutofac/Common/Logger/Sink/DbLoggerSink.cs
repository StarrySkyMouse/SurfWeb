using Serilog.Core;
using Serilog.Events;

namespace Common.Logger.Sink;

/// <summary>
///     数据库日志
/// </summary>
public class DbLoggerSink : ILogEventSink
{
    /// <summary>
    /// 属性注入
    /// </summary>
    public required IDbLoggerSinkExecute Execute { get; set; }
    public void Emit(LogEvent logEvent)
    {
        Execute.Emit(logEvent);
    }
}
/// <summary>
/// 对外提供依赖注入的实现
/// </summary>
public interface IDbLoggerSinkExecute
{
    void Emit(LogEvent logEvent);
}