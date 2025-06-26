using Core.Dto;
using Core.Dto.Players;
using Core.IServices.Base;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
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
        Task<int> GetPlayerWRCount(string id, RecordTypeEnum recordType);
        /// <summary>
        /// 获取玩家WRList
        /// </summary>
        Task<List<PlayerWRDto>> GetPlayerWRList(string id, RecordTypeEnum recordType, int pageIndex);
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
    }
}
