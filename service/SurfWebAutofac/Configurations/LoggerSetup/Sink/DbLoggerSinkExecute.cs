using Common.Logger.Sink;
using IServices.Log;
using Model.Models.Log;
using Serilog.Events;

namespace Configurations.LoggerSetup.Sink;

public class DbLoggerSinkExecute : IDbLoggerSinkExecute
{
    private readonly IDbLogServices _dbLogServices;

    public DbLoggerSinkExecute(IDbLogServices dbLogServices)
    {
        _dbLogServices = dbLogServices;
    }

    public void Emit(LogEvent logEvent)
    {
        _dbLogServices.Insert(new DbLogModel
        {
            Message = logEvent.RenderMessage()
        });
        //var message = logEvent.RenderMessage();
        //var level = logEvent.Level.ToString();
        //var timestamp = logEvent.Timestamp.UtcDateTime;
        //var properties = logEvent.Properties; // 结构化属性
    }
}