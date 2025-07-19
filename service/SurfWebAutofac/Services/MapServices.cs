using IServices;
using Model.Dtos.Maps;
using Model.Models;
using Repository.BASE;
using Services.Base;
using SqlSugar;

namespace Services
{
    public class MapServices(IBaseRepository<MapModel> mapRepository)
        : BaseServices<MapModel>(mapRepository), IMapServices
    {
        /// <summary>
        /// 获取地图列表
        /// </summary>
        public async Task<int> GetMapCount(string? difficulty, string? search)
        {
            return await GetMapQueryable(difficulty, search).CountAsync();
        }

        /// <summary>
        /// 获取地图信息
        /// </summary>
        public async Task<MapDto?> GetMapInfoById(long id)
        {
            return await _tEntityEntityRepository.Queryable()
                .Select(t => new MapDto()
                {
                    id = t.id,
                    Name = t.Name,
                    Difficulty = t.Difficulty,
                    Img = t.Img,
                    SurcessNumber = t.SurcessNumber,
                    BonusNumber = t.BonusNumber,
                    StageNumber = t.StageNumber
                }).FirstAsync(t => t.id == id);
        }

        /// <summary>
        /// 获取地图列表
        /// </summary>
        public async Task<List<MapListDto>> GetMapList(string? difficulty, string? search, int pageIndex)
        {
            return await GetMapQueryable(difficulty, search)
                .Select(t => new MapListDto()
                {
                    id = t.id,
                    Name = t.Name,
                    Difficulty = t.Difficulty,
                    Img = t.Img,
                })
                .ToPageListAsync(pageIndex, 10);
        }

        private ISugarQueryable<MapModel> GetMapQueryable(string? difficulty, string? search)
        {
            return _tEntityEntityRepository.Queryable()
                .OrderBy(t => t.Name)
                .WhereIF(!string.IsNullOrWhiteSpace(difficulty),
                    t => t.Difficulty.ToUpper() == difficulty.ToUpper().Trim())
                .WhereIF(!string.IsNullOrWhiteSpace(search), t => t.Name.ToUpper().Contains(search.ToUpper()));
        }
    }
}