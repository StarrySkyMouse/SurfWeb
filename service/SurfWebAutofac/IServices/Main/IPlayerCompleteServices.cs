using IServices.Base;
using Model.Models.Main;

namespace IServices.Main;

public interface IPlayerCompleteServices : IBaseServices<PlayerCompleteModel>
{
    /// <summary>
    ///     获取最后一次的时间
    /// </summary>
    (DateTime?, DateTime?) GetFinallyDateTime();

    /// <summary>
    ///     通过时间查询
    /// </summary>
    Task<List<PlayerCompleteModel>> GetByDate(DateTime date, List<RecordTypeEnum> typeList);

    /// <summary>
    ///     处理增量过来的数据还未完成关联的
    /// </summary>
    Task DisposeDataAssociation();

    Task HideUnLikeData();

    /// <summary>
    ///     获取旧的数据(主线和奖励)
    /// </summary>
    Task<List<PlayerCompleteModel>> GetOldPlayertimesData(IEnumerable<(int auth, string map, int track)> list);

    /// <summary>
    ///     获取旧的数据(阶段)
    /// </summary>
    Task<List<PlayerCompleteModel>> GetOldStagetimesData(IEnumerable<(int auth, string map, int stage)> list);
}