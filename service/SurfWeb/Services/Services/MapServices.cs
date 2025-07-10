using Microsoft.EntityFrameworkCore;
using Model.Cahces;
using Model.Dtos.Maps;
using Model.Entitys;
using Repositories.IRepository;
using Services.Base;
using Services.IServices;
using Utils.Extensions;

namespace Services.Services
{
    public class MapServices : BaseServices<MapModel>, IMapServices
    {
        private readonly IMapRepository _repository;
        private readonly IPlayerCompleteRepository _playerCompleteRepository;
        private readonly IMapRepository _mapRepository;
        public MapServices(IMapRepository repository,
            IPlayerCompleteRepository playerCompleteRepository,
            IMapRepository mapRepository) : base(repository)
        {
            _repository = repository;
            _playerCompleteRepository = playerCompleteRepository;
            _mapRepository = mapRepository;
        }
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
        public async Task<MapDto?> GetMapInfoById(string id)
        {
            return await _repository
                .Select(
                    t => new MapDto()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Difficulty = t.Difficulty,
                        Img = t.Img,
                        SurcessNumber = t.SurcessNumber,
                        BonusNumber = t.BonusNumber,
                        StageNumber = t.StageNumber
                    }).FirstOrDefaultAsync(t => t.Id == id);
        }
        /// <summary>
        /// 获取地图列表
        /// </summary>
        public async Task<List<MapListDto>> GetMapList(string? difficulty, string? search, int pageIndex)
        {
            return await GetMapQueryable(difficulty, search)
                .PageData(pageIndex, 10)
                .Select(t => new MapListDto()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Difficulty = t.Difficulty,
                    Img = t.Img,
                }).ToListAsync();
        }
        private IQueryable<MapModel> GetMapQueryable(string? difficulty, string? search)
        {
            return _repository
                .OrderBy(t => t.Name)
                .WhereIf(!string.IsNullOrWhiteSpace(difficulty), t => t.Difficulty.ToUpper() == difficulty.ToUpper().Trim())
                .WhereIf(!string.IsNullOrWhiteSpace(search), t => t.Name.ToUpper().Contains(search.ToUpper()));
        }

        /// <summary>
        /// 获取地图前100数量
        /// </summary>
        public async Task<int> GetMapTop100Count(string id, RecordTypeEnum recordType)
        {
            var result = await GetMapTop100Queryable(id, recordType).CountAsync();
            return result > 100 ? 100 : result;
        }
        /// <summary>
        /// 获取地图前100
        /// </summary>
        public async Task<List<MapTop100Dto>> GetMapTop100List(string id, RecordTypeEnum recordType, int pageIndex)
        {
            return await GetMapTop100Queryable(id, recordType)
                .OrderBy(t => t.Time)
                .PageData(pageIndex, 10)
                .Select(t => new MapTop100Dto()
                {
                    PlayerId = t.PlayerId,
                    PlayerName = t.PlayerName,
                    Stage = t.Stage,
                    Time = t.Time,
                    Date = t.Date,
                }).ToListAsync();
        }
        private IQueryable<PlayerCompleteModel> GetMapTop100Queryable(string id, RecordTypeEnum recordType)
        {
            return _playerCompleteRepository
                .Where(t => t.MapId == id && t.Type == recordType);
        }
        /// <summary>
        /// 获取地图缓存列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<MapCacheDto>> GetMapCacheList()
        {
            return await _repository.Select(t => new MapCacheDto()
            {
                Id = t.Id,
                Name = t.Name,
                Difficulty = t.Difficulty,
                BonusNumber = t.BonusNumber,
                StageNumber = t.StageNumber,
            }).ToListAsync();
        }
        /// <summary>
        /// 获取地图WR缓存列表
        /// </summary>
        public async Task<List<MapWrCache>> GetMapWrCacheList()
        {
            var wrList = await _playerCompleteRepository
                .GroupBy(t => new
                {
                    t.MapId,
                    t.Type,
                    t.Stage
                })
                .Select(t => new
                {
                    t.Key,
                    WR = t.OrderBy(a => a.Time).First()
                }).ToListAsync();
            var mapDifficulty = (await _mapRepository
                 .Where(t => wrList.Select(a => a.Key.MapId).Distinct().Contains(t.Id))
                 .Select(t => new
                 {
                     t.Id,
                     t.Difficulty
                 }).ToListAsync()).ToDictionary(t => t.Id);
            return wrList.Select(t => new MapWrCache()
            {
                PlayerId = t.WR.PlayerId,
                PlayerName = t.WR.PlayerName,
                MapId = t.Key.MapId,
                MapName = t.WR.MapName,
                Difficulty = mapDifficulty[t.WR.MapId].Difficulty,
                Type = t.Key.Type,
                Stage = t.Key.Stage,
                Time = t.WR.Time,
                Date = t.WR.Date
            }).ToList();
        }
        /// <summary>
        /// 通过地图名称获取地图ID列表
        /// </summary>
        public async Task<Dictionary<string, string>> GetMapIdListByName(List<string> mapNameList)
        {
            return (await _repository
               .Where(t => mapNameList.Select(a => a.Trim()).Contains(t.Name))
               .Select(t => new
               {
                   t.Id,
                   t.Name
               }).ToListAsync()).ToDictionary(t => t.Name, t => t.Id);
        }
        /// <summary>
        /// 统计地图完成人数
        /// </summary>
        public async Task UpdateSucceesNumber()
        {
            //_repository 本身虽然实现了 IQueryable<MapModel>，
            //但它并不一定直接是由 Entity Framework Core 的 DbSet<MapModel> 实例化的，
            //而可能是自定义仓储实现或经过扩展方法包装，
            //导致其类型不是 EF Core 能识别的 IQueryable 源。

            //•	加上 .Select(t => t) 后，LINQ 会生成一个新的 IQueryable，
            //其 Provider 变成了 EF Core 能识别的类型（通常是 EntityQueryProvider），
            //此时 ToListAsync() 就能正常工作。
            var mapList = await _repository.Select(t => t).ToListAsync();
            foreach (var item in mapList)
            {
                var succeesNumber = await _playerCompleteRepository
                    .Where(t => t.PlayerId != null && t.MapId == item.Id && t.Type == RecordTypeEnum.Main)
                    .Select(t => t.PlayerId).Distinct().CountAsync();
                item.SurcessNumber = succeesNumber;
                _repository.Update(item);
                _repository.SaveChanges();
            }
        }
        public async Task<List<MapModel>> GetMapInfoByNameList(IEnumerable<string> names)
        {
            return await _repository
                .Where(t => names.Select(a => a.Trim()).Contains(t.Name))
                .ToListAsync();
        }
    }
}