using IServices.Main.Base;
using Model.Dtos.Players;
using Model.Models.Main;

namespace IServices.Main;

public interface IPlayerServices : IMainBaseServices<PlayerModel>
{
    /// <summary>
    ///     获取玩家信息
    /// </summary>
    Task<PlayerInfoDto?> GetPlayerInfo(long id);

    /// <summary>
    ///     获取玩家已完成Count
    /// </summary>
    Task<int> GetPlayerSucceesCount(long id, RecordTypeEnum recordType, string difficulty);

    /// <summary>
    ///     获取玩家已完成List
    /// </summary>
    Task<List<PlayerSucceesDto>> GetPlayerSucceesList(long id, RecordTypeEnum recordType, string difficulty,
        int pageIndex);

    /// <summary>
    ///     获取玩家未完成Count
    /// </summary>
    Task<int> GetPlayerFailCount(long id, RecordTypeEnum recordType, string difficulty);

    /// <summary>
    ///     获取玩家未完成List
    /// </summary>
    Task<List<PlayerFailDto>> GetPlayerFailList(long id, RecordTypeEnum recordType, string difficulty, int pageIndex);

    /// <summary>
    ///     获取玩家列表分页数据
    /// </summary>
    Task<List<PlayerModel>> GetPlayerPageList(int pageIndex, int pageSize);

    /// <summary>
    ///     通过Auth获取(玩家Id,玩家名称)列表
    /// </summary>
    Task<Dictionary<int, (long, string)>> GetPlayerInfoListByAuth(List<int> authList);

    /// <summary>
    ///     更新玩家信息
    /// </summary>
    Task UpdateStatsNumber();

    /// <summary>
    ///     修改信息
    /// </summary>
    Task ChangeInfo(List<PlayerModel> changeList);

    /// <summary>
    ///     通过玩家名称获取玩家信息
    /// </summary>
    Task<List<PlayerModel>> GetPlayersByNames(List<string> names);
}