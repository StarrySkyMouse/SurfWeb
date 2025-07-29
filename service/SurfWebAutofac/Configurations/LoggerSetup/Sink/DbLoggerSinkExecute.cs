using Common.Logger.Sink;
using IServices.Log;
using Microsoft.Extensions.DependencyInjection;
using Model.Models.Log;
using Serilog.Events;

namespace Configurations.LoggerSetup.Sink;

public class DbLoggerSinkExecute : IDbLoggerSink
{
    private readonly IServiceProvider _serviceProvider;

    public DbLoggerSinkExecute(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        // 每次 Emit 时，从容器获取最新的服务实例（防止生命周期冲突）
        using var scope = _serviceProvider.CreateScope();
        var dbLogServices = scope.ServiceProvider.GetRequiredService<IDbLogServices>();
        dbLogServices.Insert(new DbLogModel
        {
            Message = logEvent.RenderMessage()
        });
    }
}