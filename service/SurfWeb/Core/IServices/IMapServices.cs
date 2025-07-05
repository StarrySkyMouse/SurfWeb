using Core.Dto.Maps;
using Core.IServices.Base;
using Core.Models;
using Core.Utils.Cahces;

namespace Core.IServices
{
    public interface IMapServices : IBaseServices<MapModel>
    {
        /// <summary>
        /// 获取地图信息
        /// </summary>
        Task<MapDto?> GetMapInfoById(string id);
        Task<List<MapModel>> GetMapInfoByNameList(IEnumerable<string> names);
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
        /// <summary>
        /// 获取地图WR缓存列表
        /// </summary>
        Task<List<MapWrCache>> GetMapWrCacheList();
        /// <summary>
        /// 通过地图名称获取地图ID列表
        /// </summary>
        Task<Dictionary<string, string>> GetMapIdListByName(List<string> mapNameList);
        /// <summary>
        /// 统计地图完成人数
        /// </summary>
        Task UpdateSucceesNumber();
    }
}