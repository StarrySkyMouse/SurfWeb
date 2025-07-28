using IServices.Main;
using Model.Dtos.Maps;
using Model.Models.Main;
using Services.Base;
using SqlSugar;
using Common.Logger.AOP;
using Common.Logger.AOP.Cache;
using Repository.BASE.Main;

namespace Services.Main;

public class MapServices : BaseServices<MapModel>, IMapServices
{
    private readonly IMainRepository<MapModel> _mapRepository;
    private readonly IMainRepository<PlayerCompleteModel> _playerCompleteRepository;

    public MapServices(IMainRepository<MapModel> mapRepository,
        IMainRepository<PlayerCompleteModel> playerCompleteRepository)
    {
        _mapRepository = mapRepository;
        _playerCompleteRepository = playerCompleteRepository;
    }

    /// <summary>
    ///     获取地图信息
    /// </summary>
    //[Cache(CacheTime = 1800)]
    public async Task<MapDto?> GetMapInfoById(long id)
    {
        return await _mapRepository.Queryable()
            .Select(t => new MapDto
            {
                Id = t.Id,
                Name = t.Name,
                Difficulty = t.Difficulty,
                Img = t.Img,
                SurcessNumber = t.SurcessNumber,
                BonusNumber = t.BonusNumber,
                StageNumber = t.StageNumber
            }).FirstAsync(t => t.Id == id);
    }

    /// <summary>
    ///获取地图列表
    /// </summary>
    [Cache]
    public async Task<int> GetMapCount(string? difficulty, string? search)
    {
        return await GetMapQueryable(difficulty, search).CountAsync();
    }

    /// <summary>
    ///获取地图列表
    /// </summary>
    [Cache]
    public async Task<List<MapListDto>> GetMapList(string? difficulty, string? search, int pageIndex)
    {
        return await GetMapQueryable(difficulty, search)
            .Select(t => new MapListDto
            {
                Id = t.Id,
                Name = t.Name,
                Difficulty = t.Difficulty,
                Img = t.Img
            })
            .ToPageListAsync(pageIndex, 10);
    }

    /// <summary>
    ///获取地图前100数量
    /// </summary>
    [Cache]
    public async Task<int> GetMapTop100Count(long id, RecordTypeEnum recordType, int? stage)
    {
        var result = await GetMapTop100Queryable(id, recordType, stage).CountAsync();
        return result > 100 ? 100 : result;
    }

    /// <summary>
    ///获取地图前100
    /// </summary>
    [Cache]
    public Task<List<MapTop100Dto>> GetMapTop100List(long id, RecordTypeEnum recordType, int? stage, int pageIndex)
    {
        throw new AbandonedMutexException();
    }

    /// <summary>
    ///     获取地图缓存列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<MapCacheDto>> GetMapCacheList()
    {
        throw new AbandonedMutexException();
    }

    ///// <summary>
    ///// 获取地图WR缓存列表
    ///// </summary>
    //public async Task<List<MapWrCache>> GetMapWrCacheList()
    //{
    //    var wrList = await _playerCompleteRepository
    //        .GroupBy(t => new
    //        {
    //            t.MapId,
    //            t.Type,
    //            t.Stage
    //        })
    //        .Select(t => new
    //        {
    //            t.Key,
    //            WR = t.OrderBy(a => a.Time).First()
    //        }).ToListAsync();
    //    var mapDifficulty = (await _mapRepository
    //         .Where(t => wrList.Select(a => a.Key.MapId).Distinct().Contains(t.Id))
    //         .Select(t => new
    //         {
    //             t.Id,
    //             t.Difficulty,
    //             t.Img
    //         }).ToListAsync()).ToDictionary(t => t.Id);
    //    return wrList.Select(t => new MapWrCache()
    //    {
    //        PlayerId = t.WR.PlayerId,
    //        PlayerName = t.WR.PlayerName,
    //        MapId = t.Key.MapId,
    //        MapName = t.WR.MapName,
    //        Difficulty = mapDifficulty[t.WR.MapId].Difficulty,
    //        Img = mapDifficulty[t.WR.MapId].Img,
    //        Type = t.Key.Type,
    //        Stage = t.Key.Stage,
    //        Time = t.WR.Time,
    //        Date = t.WR.Date
    //    }).ToList();
    //}
    /// <summary>
    ///     通过地图名称获取地图ID列表
    /// </summary>
    public async Task<Dictionary<string, long>> GetMapIdListByName(List<string> mapNameList)
    {
        //return (await _mapRepository.Queryable()
        //   .Where(t => mapNameList.Select(a => a.Trim()).Contains(t.Name))
        //   .Select(t => new
        //   {
        //       t.Id,
        //       t.Name
        //   }).ToListAsync()).ToDictionary(t => t.Name, t => t.Id);
        throw new NotImplementedException("请实现 GetMapIdListByName 方法");
    }

    /// <summary>
    ///     统计地图完成人数
    /// </summary>
    public async Task UpdateSucceesNumber()
    {
        //_mapRepository 本身虽然实现了 IQueryable<MapModel>，
        //但它并不一定直接是由 Entity Framework Core 的 DbSet<MapModel> 实例化的，
        //而可能是自定义仓储实现或经过扩展方法包装，
        //导致其类型不是 EF Core 能识别的 IQueryable 源。

        //•	加上 .Select(t => t) 后，LINQ 会生成一个新的 IQueryable，
        //其 Provider 变成了 EF Core 能识别的类型（通常是 EntityQueryProvider），
        //此时 ToListAsync() 就能正常工作。
        var mapList = await _mapRepository.Queryable().Select(t => t).ToListAsync();
        foreach (var item in mapList)
        {
            var succeesNumber = await _playerCompleteRepository.Queryable()
                .Where(t => t.PlayerId != 0 && t.MapId == item.Id && t.Type == RecordTypeEnum.Main)
                .Select(t => t.PlayerId).Distinct().CountAsync();
            item.SurcessNumber = succeesNumber;
            _mapRepository.Update(item);
        }
    }

    public async Task<MapModel?> GetMapInfoByName(string names)
    {
        return await _mapRepository.Queryable()
            .Where(t => t.Name.Trim().ToUpper() == names.Trim().ToUpper())
            .FirstAsync();
    }

    public async Task<List<MapModel>> GetMapInfoByNameList(IEnumerable<string> names)
    {
        return await _mapRepository.Queryable()
            .Where(t => names.Select(a => a.Trim()).Contains(t.Name))
            .ToListAsync();
    }

    private ISugarQueryable<MapModel> GetMapQueryable(string? difficulty, string? search)
    {
        return _mapRepository.Queryable()
            .OrderBy(t => t.Name)
            .WhereIF(!string.IsNullOrWhiteSpace(difficulty),
                t => t.Difficulty.ToUpper() == difficulty.ToUpper().Trim())
            .WhereIF(!string.IsNullOrWhiteSpace(search), t => t.Name.ToUpper().Contains(search.ToUpper()));
    }

    private ISugarQueryable<PlayerCompleteModel> GetMapTop100Queryable(long id, RecordTypeEnum recordType, int? stage)
    {
        return _playerCompleteRepository.Queryable()
            .Where(t => t.MapId == id && t.Type == recordType)
            .WhereIF(stage.HasValue && recordType != RecordTypeEnum.Main, t => t.Stage == stage);
    }
}