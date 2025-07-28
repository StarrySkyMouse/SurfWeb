using Serilog.Core;
using Serilog.Events;

namespace Common.Logger.Sink;

/// <summary>
///     服务端接口日志
/// </summary>
public interface IServiceLoggerSink : ILogEventSink
{
}