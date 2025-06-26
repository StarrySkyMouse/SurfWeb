using Core.Dto;
using Core.Dto.Maps;
using Core.IServices.Base;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IMapServices : IBaseServices<MapModel>
    {
        /// <summary>
        /// 获取地图信息
        /// </summary>
        Task<MapDto?> GetMapInfo(string id);
        /// <summary>
        /// 获取地图列表
        /// </summary>
        Task<int> GetMapCount(string? difficulty, string? search);
        /// <summary>
        /// 获取地图列表
        /// </summary>
        Task<List<MapListDto>> GetMapList(string? difficulty, string? search, int pageIndex);
        /// <summary>
        /// 获取地图前100数量
        /// </summary>
        Task<int> GetMapTop100Count(string id, RecordTypeEnum recordType);
        /// <summary>
        /// 获取地图前100
        /// </summary>
        Task<List<MapTop100Dto>> GetMapTop100List(string id, RecordTypeEnum recordType, int pageIndex);
        /// <summary>
        /// 获取地图缓存列表
        /// </summary>
        Task<List<MapCacheDto>> GetMapCacheList();
    }
}
