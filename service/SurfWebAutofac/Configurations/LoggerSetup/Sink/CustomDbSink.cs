using Microsoft.Data.SqlClient;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Configurations.LoggerSetup.Sink
{
    public class CustomDbSink : ILogEventSink
    {
        public CustomDbSink()
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
