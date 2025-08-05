using Common.Caches.AOP;
using Common.Caches.Base;
using Common.Db.SqlSugar.Repository.Main;
using IServices.Main;
using Model.Caches;
using Model.Dtos.Maps;
using Model.Dtos.NewRecords;
using Model.Models.Main;
using Services.Base;
using Services.Main.ExtensionsRepository;
using SqlSugar;
using Utils.Extensions;

namespace Services.Main;

//设置缓存2分钟
[Cache(CacheTime = 120)]
public class MapServices : BaseServices<MapModel>, IMapServices
{
    private readonly IMainRepository<MapModel> _mapRepository;
    private readonly PlayerCompleteRepository _playerCompleteRepository;

    public MapServices(IMainRepository<MapModel> mapRepository,
        PlayerCompleteRepository playerCompleteRepository,
        ICache cache) : base(mapRepository)
    {
        _mapRepository = mapRepository;
        _playerCompleteRepository = playerCompleteRepository;
    }

    /// <summary>
    ///     获取地图信息
    /// </summary>
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
    ///     获取地图列表
    /// </summary>
    public async Task<int> GetMapCount(string? difficulty, string? search)
    {
        return await GetMapQueryable(difficulty, search).CountAsync();
    }

    /// <summary>
    ///     获取地图列表
    /// </summary>
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
            .Where(t => t.MapId == id)
            .WhereIf(stage.HasValue && recordType != RecordTypeEnum.Main, t => t.Stage == stage)
            .FirstOrDefault();
        var result = await GetMapTop100Queryable(id, recordType, stage)
            .OrderBy(t => t.Time).Select(t => new MapTop100Dto
            {
                Ranking = 0,
                PlayerId = t.PlayerId,
                PlayerName = t.PlayerName,
                Stage = t.Stage,
                Time = t.Time,
                Date = t.Date
            })
            .ToPageListAsync(pageIndex, 10);
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
        return (await _mapRepository.Queryable()
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
        var mapList = await _mapRepository.Queryable().Select(t => t).ToListAsync();
        foreach (var item in mapList)
        {
            var succeesNumber = await _playerCompleteRepository.QueryableNoHide()
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

    //获取地图Wr信息(缓存10分钟)
    //[Cache(CacheTime = 10 * 60)]
    public async Task<List<MapWrCache>> GetMapWrList(RecordTypeEnum recordType)
    {
        var list = await _playerCompleteRepository.Queryable()
            .Where(t => t.Type == recordType)
            .GroupBy(t => new { t.MapId, t.Stage })
            .Select(t => new
            {
                Time = SqlFunc.AggregateMin(t.Time),
                t.MapId,
                t.Stage
            })
            .InnerJoin(_playerCompleteRepository.Queryable(), (a, b) => a.MapId == b.MapId && a.Time == b.Time)
            .Select((a, b) => new
            {
                a.MapId,
                b.MapName,
                a.Stage,
                a.Time,
                b.PlayerName,
                b.PlayerId
            }).ToListAsync();
        return list.Select(t => new MapWrCache
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

    /// <summary>
    ///     获取地图信息
    /// </summary>
    public async Task<List<MapMainCache>> GetMapMainList()
    {
        return await _mapRepository.Queryable()
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

    /// <summary>
    ///     获取地图信息Bounty
    /// </summary>
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

    /// <summary>
    ///     获取地图信息Bounty
    /// </summary>
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


    /// <summary>
    ///     获取最新纪录
    /// </summary>
    public async Task<List<NewRecordDto>> GetNewRecordList(RecordTypeEnum recordType)
    {
        var result = new List<NewRecordDto>();
        var i = 1;
        var isQuit = false;
        while (true)
        {
            var list = await _playerCompleteRepository.Queryable().Select(t => t)
                .Where(t => t.Type == recordType)
                .OrderByDescending(t => t.Date)
                .ToPageListAsync(i, 100);
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
        var mapInfo = await _mapRepository.Queryable()
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

    /// <summary>
    ///     获取新增地图
    /// </summary>
    public async Task<List<NewMapDto>> GetNewMapList()
    {
        return await _mapRepository.Queryable().Select(t => new NewMapDto
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

    /// <summary>
    ///     热门地图
    /// </summary>
    public async Task<List<PopularMapDto>> GetPopularMapList()
    {
        var result = await _mapRepository.Queryable().Select(t => new PopularMapDto
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