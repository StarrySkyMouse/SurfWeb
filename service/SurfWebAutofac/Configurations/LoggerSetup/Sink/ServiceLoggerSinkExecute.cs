using Common.Logger.Sink;
using IServices.Log;
using Model.Models.Log;
using Serilog.Events;

namespace Configurations.LoggerSetup.Sink;

public class ServiceLoggerSinkExecute : IServiceLoggerSinkExecute
{
    private readonly IServicesLogServices _servicesLogServices;

    public ServiceLoggerSinkExecute(IServicesLogServices servicesLogServices)
    {
        _servicesLogServices = servicesLogServices;
    }

    public void Emit(LogEvent logEvent)
    {
        _servicesLogServices.Insert(new ServicesLogModel
        {
            Message = logEvent.RenderMessage()
        });
    }
}