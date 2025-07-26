using Common.Quartz.Base;
using IServices.Main;
using Quartz;
using Repository.Other;

namespace Configurations.QuartzSetup.SequenceJobs;

/// <summary>
///     地图同步后
/// </summary>
public class MapLaterSequenceJob : ISequenceJob
{
    private readonly IMapServices _mapServices;
    private readonly ISqlHelp _sqlHelp;

    public MapLaterSequenceJob(
        ISqlHelp sqlHelp,
        IMapServices mapServices
    )
    {
        _sqlHelp = sqlHelp;
        _mapServices = mapServices;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        //更新地图完成人数
        await _mapServices.UpdateSucceesNumber();
    }
}