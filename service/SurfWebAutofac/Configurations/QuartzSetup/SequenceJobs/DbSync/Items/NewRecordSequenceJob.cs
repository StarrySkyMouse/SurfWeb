using Common.Dapper;
using Common.Quartz.Base;
using IServices.Main;
using Quartz;

namespace Configurations.QuartzSetup.SequenceJobs.DbSync.Items;

/// <summary>
///     新记录同步
/// </summary>
public class NewRecordSequenceJob : ISequenceJob
{
    private readonly INewRecordServices _newRecordServices;

    public NewRecordSequenceJob(
        ISqlHelp sqlHelp,
        INewRecordServices newRecordServices)
    {
        _newRecordServices = newRecordServices;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        //更新新的
        //await _newRecordServices.UpdateNewRecord();
    }
}