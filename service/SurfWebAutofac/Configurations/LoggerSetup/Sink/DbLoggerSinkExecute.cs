using Common.Logger.Sink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;

namespace Configurations.LoggerSetup.Sink
{
    public class DbLoggerSinkExecute: IDbLoggerSinkExecute
    {
        public DbLoggerSinkExecute()
        {

        }
        public void Emit(LogEvent logEvent)
        {
            // 1. 格式化日志数据
            var message = logEvent.RenderMessage();
            var level = logEvent.Level.ToString();
            var timestamp = logEvent.Timestamp.UtcDateTime;
            var properties = logEvent.Properties; // 结构化属性
        }
    }
}
