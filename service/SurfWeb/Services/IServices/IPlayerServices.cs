using Model.Dtos.Players;
using Model.Entitys;
using Services.Base;

namespace Services.IServices
{
    public interface IPlayerServices : IBaseServices<PlayerModel>
    {
        /// <summary>
        /// 获取玩家信息
        /// </summary>
        Task<PlayerInfoDto?> GetPlayerInfo(string id);
        /// <summary>
        /// 获取玩家WRCount
        /// </summary>
        int GetPlayerWRCount(string id, RecordTypeEnum recordType);
        /// <summary>
        /// 获取玩家WRList
        /// </summary>
        List<PlayerWRDto> GetPlayerWRList(string id, RecordTypeEnum recordType, int pageIndex);
        /// <summary>
        /// 获取玩家已完成Count
        /// </summary>
        Task<int> GetPlayerSucceesCount(string id, RecordTypeEnum recordType);
        /// <summary>
        /// 获取玩家已完成List
        /// </summary>
        Task<List<PlayerSucceesDto>> GetPlayerSucceesList(string id, RecordTypeEnum recordType, int pageIndex);
        /// <summary>
        /// 获取玩家未完成Count
        /// </summary>
        Task<int> GetPlayerFailCount(string id, RecordTypeEnum recordType);
        /// <summary>
        /// 获取玩家未完成List
        /// </summary>
        Task<List<PlayerFailDto>> GetPlayerFailList(string id, RecordTypeEnum recordType, int pageIndex);
        /// <summary>
        /// 获取玩家列表分页数据
        /// </summary>
        Task<List<PlayerModel>> GetPlayerPageList(int pageIndex, int pageSize);
        /// <summary>
        /// 通过Auth获取(玩家Id,玩家名称)列表
        /// </summary>
        Task<Dictionary<int, (string, string)>> GetPlayerInfoListByAuth(List<int> authList);
        /// <summary>
        /// 更新玩家信息
        /// </summary>
        Task UpdateStatsNumber();
        /// <summary>
        /// 修改信息
        /// </summary>
        Task ChangeInfo(List<PlayerModel> changeList);
    }
}
