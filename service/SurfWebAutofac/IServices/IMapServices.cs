using IServices.Base;
using Model.Dtos.Maps;
using Model.Models;

namespace IServices
{
    public interface IMapServices : IBaseServices<MapModel>
    {
        /// <summary>
        /// 获取地图信息
        /// </summary>
        Task<MapDto?> GetMapInfoById(long id);
        /// <summary>
        /// 获取地图列表
        /// </summary>
        Task<int> GetMapCount(string? difficulty, string? search);
        /// <summary>
        /// 获取地图列表
        /// </summary>
        Task<List<MapListDto>> GetMapList(string? difficulty, string? search, int pageIndex);
    }
}