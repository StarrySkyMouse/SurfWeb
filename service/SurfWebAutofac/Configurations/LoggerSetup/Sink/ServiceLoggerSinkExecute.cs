using Common.Logger.Sink;
using IServices.Log;
using Microsoft.Extensions.DependencyInjection;
using Model.Models.Log;
using Serilog.Events;

namespace Configurations.LoggerSetup.Sink;

public class ServiceLoggerSinkExecute : IServiceLoggerSink
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceLoggerSinkExecute(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        using var scope = _serviceProvider.CreateScope();
        var servicesLogServices = scope.ServiceProvider.GetRequiredService<IServicesLogServices>();
        servicesLogServices.Insert(new ServicesLogModel
        {
            Message = logEvent.RenderMessage()
        });
        //var message = logEvent.RenderMessage();
        //var level = logEvent.Level.ToString();
        //var timestamp = logEvent.Timestamp.UtcDateTime;
        //var properties = logEvent.Properties; // 结构化属性
    }
}