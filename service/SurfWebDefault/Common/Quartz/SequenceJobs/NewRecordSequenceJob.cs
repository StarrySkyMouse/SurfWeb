using Quartz;
using Repositories.Exterior;
using Services.IServices;

namespace Common.Quartz.SequenceJobs;

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
        await _newRecordServices.UpdateNewRecord();
    }
}