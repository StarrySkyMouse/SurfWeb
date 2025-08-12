using Common.Db.EFCore.Repository;
using EFServices.Base;
using IServices.Main;
using Microsoft.EntityFrameworkCore;
using Model.Caches;
using Model.Dtos.Maps;
using Model.Dtos.NewRecords;
using Model.Models.Main;
using Utils.Extensions;

namespace EFServices;

public class MapServices : BaseServices<MapModel>, IMapServices
{
    private readonly IBaseRepository<MapModel> _mapRepository;
    private readonly IBaseRepository<PlayerCompleteModel> _playerCompleteRepository;

    public MapServices(IBaseRepository<MapModel> mapRepository,
        IBaseRepository<PlayerCompleteModel> playerCompleteRepository) : base(mapRepository)
    {
        _mapRepository = mapRepository;
        _playerCompleteRepository = playerCompleteRepository;
    }

    /// <summary>
    ///     获取地图列表
    /// </summary>
    public async Task<int> GetMapCount(string? difficulty, string? search)
    {
        return await GetMapQueryable(difficulty, search).CountAsync();
    }

    /// <summary>
    ///     获取地图信息
    /// </summary>
    public async Task<MapDto?> GetMapInfoById(long id)
    {
        return await _mapRepository
            .Select(t => new MapDto
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
    ///     获取地图列表
    /// </summary>
    public async Task<List<MapListDto>> GetMapList(string? difficulty, string? search, int pageIndex)
    {
        return await GetMapQueryable(difficulty, search)
            .PageData(pageIndex, 10)
            .Select(t => new MapListDto
            {
                Id = t.Id,
                Name = t.Name,
                Difficulty = t.Difficulty,
                Img = t.Img
            }).ToListAsync();
    }

    /// <summary>
    ///     获取地图前100数量
    /// </summary>
    public async Task<int> GetMapTop100Count(long id, RecordTypeEnum recordType, int? stage)
    {
        var result = await GetMapTop100Queryable(id, recordType, stage).CountAsync();
        return result > 100 ? 100 : result;
    }

    /// <summary>
    ///     获取地图前100
    /// </summary>
    public async Task<List<MapTop100Dto>> GetMapTop100List(long id, RecordTypeEnum recordType, int? stage,
        int pageIndex)
    {
        var mapWr = (await GetMapWrList(recordType))
            .Where(t => t.MapId == id && t.Type == recordType)
            .WhereIf(stage.HasValue && recordType != RecordTypeEnum.Main, t => t.Stage == stage)
            .FirstOrDefault();
        var result = await GetMapTop100Queryable(id, recordType, stage)
            .OrderBy(t => t.Time)
            .PageData(pageIndex, 10)
            .Select(t => new MapTop100Dto
            {
                Ranking = 0,
                PlayerId = t.PlayerId,
                PlayerName = t.PlayerName,
                Stage = t.Stage,
                Time = t.Time,
                Date = t.Date
            }).ToListAsync();
        if (result.Any())
            for (var i = 0; i < result.Count; i++)
            {
                result[i].Ranking = (pageIndex - 1) * 10 + i + 1;
                if (mapWr != null) result[i].GapTime = mapWr.Time - result[i].Time;
            }

        return result;
    }

    /// <summary>
    ///     通过地图名称获取地图ID列表
    /// </summary>
    public async Task<Dictionary<string, long>> GetMapIdListByName(List<string> mapNameList)
    {
        return (await _mapRepository
            .Where(t => mapNameList.Select(a => a.Trim()).Contains(t.Name))
            .Select(t => new
            {
                t.Id,
                t.Name
            }).ToListAsync()).ToDictionary(t => t.Name, t => t.Id);
    }

    /// <summary>
    ///     统计地图完成人数
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
        var mapList = await _mapRepository.Select(t => t).ToListAsync();
        foreach (var item in mapList)
        {
            var succeesNumber = await _playerCompleteRepository
                .Where(t => t.PlayerId != null && t.MapId == item.Id && t.Type == RecordTypeEnum.Main)
                .Select(t => t.PlayerId).Distinct().CountAsync();
            item.SurcessNumber = succeesNumber;
            _mapRepository.Update(item);
            _mapRepository.SaveChanges();
        }
    }

    public async Task<MapModel?> GetMapInfoByName(string names)
    {
        return await _mapRepository
            .Where(t => t.Name.Trim().ToUpper() == names.Trim().ToUpper())
            .FirstOrDefaultAsync();
    }

    public async Task<List<MapModel>> GetMapInfoByNameList(IEnumerable<string> names)
    {
        return await _mapRepository
            .Where(t => names.Select(a => a.Trim()).Contains(t.Name))
            .ToListAsync();
    }

    public async Task<List<MapWrCache>> GetMapWrList(RecordTypeEnum recordType)
    {
        var minTimes = _playerCompleteRepository
            .Where(t => t.Type == recordType)
            .GroupBy(t => new { t.MapId, t.Stage })
            .Select(g => new
            {
                g.Key.MapId,
                g.Key.Stage,
                Time = g.Min(x => x.Time)
            });

        var query = minTimes
            .Join(_playerCompleteRepository,
                a => new { a.MapId,a.Stage, a.Time },
                b => new { b.MapId, b.Stage, b.Time },
                (a, b) => new
                {
                    a.MapId,
                    b.MapName,
                    a.Stage,
                    a.Time,
                    b.PlayerName,
                    b.PlayerId
                });
        var result = await query.ToListAsync();
        return result.Select(t => new MapWrCache
        {
            MapId = t.MapId,
            MapName = t.MapName,
            PlayerId = t.PlayerId,
            PlayerName = t.PlayerName,
            Time = t.Time,
            Type = recordType,
            Stage = t.Stage
        }).ToList();
    }

    public async Task<List<MapMainCache>> GetMapMainList()
    {
        return await _mapRepository
            .Select(t => new MapMainCache
            {
                Id = t.Id,
                Name = t.Name,
                Difficulty = t.Difficulty,
                Img = t.Img,
                BonusNumber = t.BonusNumber,
                StageNumber = t.StageNumber
            }).ToListAsync();
    }

    public async Task<List<MapBountyOrStageCache>> GetMapBountyList()
    {
        return (await GetMapMainList())
            .Where(t => t.BonusNumber != 0)
            .SelectMany(t =>
                Enumerable.Range(1, t.BonusNumber)
                    .Select(b => new MapBountyOrStageCache
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Difficulty = t.Difficulty,
                        Img = t.Img,
                        Stage = b
                    })).ToList();
    }

    public async Task<List<MapBountyOrStageCache>> GetMapStageList()
    {
        return (await GetMapMainList())
            .Where(t => t.StageNumber != 0)
            .SelectMany(t =>
                Enumerable.Range(1, t.StageNumber)
                    .Select(b => new MapBountyOrStageCache
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Difficulty = t.Difficulty,
                        Img = t.Img,
                        Stage = b
                    })).ToList();
    }

    public async Task<List<NewRecordDto>> GetNewRecordList(RecordTypeEnum recordType)
    {
        var result = new List<NewRecordDto>();
        var i = 1;
        var isQuit = false;
        while (true)
        {
            var list = await _playerCompleteRepository.Select(t => t)
                .Where(t => t.Type == recordType)
                .OrderByDescending(t => t.Date)
                .PageData(i, 100)
                .ToListAsync();
            i++;
            if (!list.Any()) break;

            foreach (var t in list)
            {
                var item = result.FirstOrDefault(a => a.MapId == t.MapId);
                if (item == null)
                    result.Add(new NewRecordDto
                    {
                        MapId = t.MapId,
                        MapName = t.MapName,
                        Players = new List<NewRecordDto_Player>
                        {
                            new()
                            {
                                PlayerId = t.PlayerId,
                                PlayerName = t.PlayerName,
                                Stage = t.Stage,
                                Date = t.Date,
                                Time = t.Time
                            }
                        }
                    });
                else
                    item.Players.Add(new NewRecordDto_Player
                    {
                        PlayerId = t.PlayerId,
                        PlayerName = t.PlayerName,
                        Stage = t.Stage,
                        Date = t.Date,
                        Time = t.Time
                    });

                if (result.Count == 11)
                {
                    result.Remove(result[10]);
                    isQuit = true;
                    break;
                }
            }

            if (isQuit) break;
        }

        //查询地图信息
        var mapInfo = await _mapRepository
            .Where(t => result.Select(a => a.MapId).Contains(t.Id))
            .Select(t => new
            {
                t.Id,
                t.Difficulty,
                t.Img
            })
            .ToListAsync();
        var mapWrList = (await GetMapWrList(recordType)).Where(t =>
            result.Select(a => a.MapId).Contains(t.MapId));
        //填充地图信息
        foreach (var item in result)
        {
            var map = mapInfo.FirstOrDefault(t => t.Id == item.MapId);
            if (map != null)
            {
                item.Difficulty = map.Difficulty;
                item.Img = map.Img;
            }

            item.Players.ForEach(t =>
            {
                t.GapTime = mapWrList.Any(b => b.MapId == item.MapId && b.Stage == t.Stage)
                    ? mapWrList.FirstOrDefault(b => b.MapId == item.MapId && b.Stage == t.Stage).Time - t.Time
                    : -1;
            });
            item.Players = item.Players.OrderBy(t => t.PlayerName).ThenBy(t => t.Stage).ToList();
        }

        return result;
    }

    public async Task<List<NewMapDto>> GetNewMapList()
    {
        return await _mapRepository.Select(t => new NewMapDto
            {
                Id = t.Id,
                Name = t.Name,
                Difficulty = t.Difficulty,
                Img = t.Img,
                CreateTime = t.CreateTime
            })
            .OrderByDescending(t => t.CreateTime)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<PopularMapDto>> GetPopularMapList()
    {
        var result = await _mapRepository.Select(t => new PopularMapDto
            {
                Id = t.Id,
                Name = t.Name,
                Img = t.Img,
                Difficulty = t.Difficulty,
                SurcessNumber = t.SurcessNumber
            })
            .OrderByDescending(t => t.SurcessNumber)
            .Take(10)
            .ToListAsync();
        return result;
    }

    /// <summary>
    ///     获取地图缓存列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<MapCacheDto>> GetMapCacheList()
    {
        return await _mapRepository.Select(t => new MapCacheDto
        {
            Id = t.Id,
            Name = t.Name,
            Difficulty = t.Difficulty,
            Img = t.Img,
            BonusNumber = t.BonusNumber,
            StageNumber = t.StageNumber
        }).ToListAsync();
    }

    /// <summary>
    ///     获取地图WR缓存列表
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
                t.Difficulty,
                t.Img
            }).ToListAsync()).ToDictionary(t => t.Id);
        return wrList.Select(t => new MapWrCache
        {
            PlayerId = t.WR.PlayerId,
            PlayerName = t.WR.PlayerName,
            MapId = t.Key.MapId,
            MapName = t.WR.MapName,
            Difficulty = mapDifficulty[t.WR.MapId].Difficulty,
            Img = mapDifficulty[t.WR.MapId].Img,
            Type = t.Key.Type,
            Stage = t.Key.Stage,
            Time = t.WR.Time,
            Date = t.WR.Date
        }).ToList();
    }

    private IQueryable<MapModel> GetMapQueryable(string? difficulty, string? search)
    {
        return _mapRepository
            .OrderBy(t => t.Name)
            .WhereIf(!string.IsNullOrWhiteSpace(difficulty), t => t.Difficulty.ToUpper() == difficulty.ToUpper().Trim())
            .WhereIf(!string.IsNullOrWhiteSpace(search), t => t.Name.ToUpper().Contains(search.ToUpper()));
    }

    private IQueryable<PlayerCompleteModel> GetMapTop100Queryable(long id, RecordTypeEnum recordType, int? stage)
    {
        return _playerCompleteRepository
            .Where(t => t.MapId == id && t.Type == recordType)
            .WhereIf(stage.HasValue && recordType != RecordTypeEnum.Main, t => t.Stage == stage);
    }
}