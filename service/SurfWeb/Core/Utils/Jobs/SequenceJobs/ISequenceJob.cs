using Quartz;

namespace Core.Utils.Jobs.SequenceJobs
{
    /// <summary>
    /// 顺序Job接口
    /// </summary>
    public interface ISequenceJob
    {
        /// <summary>
        /// 同步方法
        /// </summary>
        Task Execute(IJobExecutionContext context);
    }
}
