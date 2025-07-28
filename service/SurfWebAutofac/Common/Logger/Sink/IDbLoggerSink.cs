using Serilog.Core;
using Serilog.Events;

namespace Common.Logger.Sink;

/// <summary>
///     数据库日志
/// </summary>
public interface IDbLoggerSink : ILogEventSink
{
}