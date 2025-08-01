using Common.Caches.AOP;
using Common.Db.SqlSugar.Repository.Main;
using IServices.Main;
using Model.Dtos.Players;
using Model.Models.Main;
using Services.Base;
using Services.Main.ExtensionsRepository;
using SqlSugar;
using Utils.Extensions;

namespace Services.Main;

//设置缓存2分钟
//[Cache(CacheTime = 120)]
public class PlayerServices : BaseServices<PlayerModel>, IPlayerServices
{
    private readonly IMainRepository<MapModel> _mapRepository;
    private readonly PlayerCompleteRepository _playerCompleteRepository;
    private readonly IMainRepository<PlayerModel> _playerRepository;
    private readonly IMapServices _mapServices;

    public PlayerServices(IMainRepository<PlayerModel> playerRepository,
        PlayerCompleteRepository playerCompleteRepository,
        IMainRepository<MapModel> mapRepository, IMapServices mapServices) : base(playerRepository)
    {
        _playerRepository = playerRepository;
        _playerCompleteRepository = playerCompleteRepository;
        _mapRepository = mapRepository;
        _mapServices = mapServices;
    }

    /// <summary>
    ///     获取玩家信息
    /// </summary>
    public async Task<PlayerInfoDto?> GetPlayerInfo(long id)
    {
        var result = await _playerRepository.Queryable().Select(t => new PlayerInfoDto
        {
            Id = t.Id,
            Name = t.Name,
            Integral = t.Integral,
            SucceesNumber = t.SucceesNumber,
            WRNumber = t.WRNumber,
            BWRNumber = t.BWRNumber,
            SWRNumber = t.SWRNumber
        }).Where(t => t.Id == id).FirstAsync();
        if (result != null)
        {
            //积分排行
            result.IntegralRanking = await _playerRepository.Queryable()
                .Where(t => t.Integral > result.Integral)
                .CountAsync() + 1;
            //地图完成数排行
            result.SucceesRanking = await _playerRepository.Queryable()
                .Where(t => t.SucceesNumber > result.SucceesNumber)
                .CountAsync() + 1;
            //主线排行
            result.WRRanking = await _playerRepository.Queryable()
                .Where(t => t.WRNumber > result.WRNumber)
                .CountAsync() + 1;
            //奖励排行
            result.BWRanking = await _playerRepository.Queryable()
                .Where(t => t.BWRNumber > result.BWRNumber)
                .CountAsync() + 1;
            //阶段排行
            result.SWRanking = await _playerRepository.Queryable()
                .Where(t => t.SWRNumber > result.SWRNumber)
                .CountAsync() + 1;
        }

        return result;
    }

    public async Task<int> GetPlayerSucceesCount(long id, RecordTypeEnum recordType, string difficulty)
    {
        try
        {
            var result = await _playerCompleteRepository.Queryable()
                .InnerJoin(_mapRepository.Queryable().Where(t => t.Difficulty == difficulty), (a, b) => a.MapId == b.Id)
                .Select((a, b) => a)
                .OrderByDescending(a => a.Date)
                .Where(a => a.PlayerId == id && a.Type == recordType)
                .GroupBy(a => a.MapId)
                .Select(a => a.MapId)
                .CountAsync();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    ///     获取玩家已完成List
    /// </summary>
    public async Task<List<PlayerSucceesDto>> GetPlayerSucceesList(long id, RecordTypeEnum recordType,
        string difficulty, int pageIndex)
    {
        // 1. 查询玩家通关记录

        var queryKeyList = await _playerCompleteRepository.Queryable()
            .InnerJoin(
                _mapRepository.Queryable().Where(t => t.Difficulty == difficulty),
                (a, b) => a.MapId == b.Id)
            .Where(a => a.PlayerId == id && a.Type == recordType)
            .GroupBy(a => a.MapId)
            .OrderByDescending(a => a.MapId)
            .Select(a => a.MapId)
            .ToPageListAsync(pageIndex, 10);
        var queryList = (await _playerCompleteRepository.Queryable()
                .Where(t => queryKeyList.Contains(t.MapId) && t.Type == recordType && t.PlayerId == id)
                .ToListAsync())
            .OrderByDescending(t => t.Date)
            .GroupBy(t => t.MapId);
        // 2. 关联Map表，查询地图难度信息
        var mapInfoList =
            await _mapRepository.Queryable().Where(t => queryList.Select(a => a.Key).Contains(t.Id)).ToListAsync();
        //3.查询wr信息
        var mapWrList =
            (await _mapServices.GetMapWrList(recordType)).Where(t => queryList.Select(a => a.Key).Contains(t.MapId));
        //组装
        var result = queryList.Select(t => new PlayerSucceesDto
        {
            MapId = t.Key,
            MapName = t.First().MapName,
            Difficulty = mapInfoList.First(a => a.Id == t.Key).Difficulty,
            Img = mapInfoList.First(a => a.Id == t.Key).Img,
            Stages = t.Select(a => new PlayerStageDto
            {
                Stage = a.Stage,
                Time = a.Time,
                GapTime = mapWrList.Any(b => b.MapId == t.Key && b.Stage == a.Stage)
                    ? (mapWrList.FirstOrDefault(b => b.MapId == t.Key && b.Stage == a.Stage).Time - a.Time)
                    : -1,
                Date = a.Date
            }).OrderBy(a => a.Stage).ToList()
        }).ToList();
        return result;
    }

    /// <summary>
    ///     获取玩家未完成List
    /// </summary>
    public async Task<List<PlayerFailDto>> GetPlayerFailList(long id, RecordTypeEnum recordType, string difficulty,
        int pageIndex)
    {
        if (recordType == RecordTypeEnum.Main)
        {
            var list = await _playerCompleteRepository.Queryable()
                .InnerJoin(_mapRepository.Queryable().Where(t => t.Difficulty == difficulty), (a, b) => a.MapId == b.Id)
                .Where(a => a.PlayerId == id && a.Type == recordType)
                .Select(a => a.MapId).ToListAsync();
            return (await _mapServices.GetMapMainList())
                .Where(t => !list.Contains(t.Id) && t.Difficulty == difficulty)
                .OrderBy(t => t.Name)
                .Skip((pageIndex - 1) * 10)
                .Take(10)
                .Select(t => new PlayerFailDto()
                {
                    MapId = t.Id,
                    MapName = t.Name,
                    Difficulty = t.Difficulty,
                    Img = t.Img
                }).ToList();
        }
        else if (recordType == RecordTypeEnum.Bounty)
        {
            var list = await _playerCompleteRepository.Queryable()
                .InnerJoin(_mapRepository.Queryable().Where(t => t.Difficulty == difficulty), (a, b) => a.MapId == b.Id)
                .Where(a => a.PlayerId == id && a.Type == recordType)
                .Select(a => a.MapId + "@@" + a.Stage).ToListAsync();

            return (await _mapServices.GetMapBountyList())
                //获取未完成的阶段
                .Where(t => !list.Contains(t.Id + "@@" + t.Stage) && t.Difficulty == difficulty)
                .GroupBy(t => t.Id)
                .OrderBy(t => t.Key)
                .PageData(pageIndex, 10)
                .Select(t => new PlayerFailDto()
                {
                    MapId = t.Key,
                    MapName = t.First().Name,
                    Difficulty = t.First().Difficulty,
                    Img = t.First().Img,
                    Stages = t.Select(a => a.Stage).ToList()
                }).ToList();
        }
        else if (recordType == RecordTypeEnum.Stage)
        {
            var list = await _playerCompleteRepository.Queryable()
                .InnerJoin(_mapRepository.Queryable().Where(t => t.Difficulty == difficulty), (a, b) => a.MapId == b.Id)
                .Where(a => a.PlayerId == id && a.Type == recordType)
                .Select(a => a.MapId + "@@" + a.Stage).ToListAsync();
            return (await _mapServices.GetMapStageList())
                //获取未完成的阶段
                .Where(t => !list.Contains(t.Id + "@@" + t.Stage) && t.Difficulty == difficulty)
                .GroupBy(t => t.Id)
                .OrderBy(t => t.Key)
                .PageData(pageIndex, 10)
                .Select(t => new PlayerFailDto()
                {
                    MapId = t.Key,
                    MapName = t.First().Name,
                    Difficulty = t.First().Difficulty,
                    Img = t.First().Img,
                    Stages = t.Select(a => a.Stage).ToList()
                }).ToList();
        }

        return await Task.FromResult(new List<PlayerFailDto>());
    }
    //分页查询玩家数据
    public async Task<List<PlayerModel>> GetPlayerPageList(int pageIndex, int pageSize)
    {
        return await _playerRepository.Queryable()
            .OrderByDescending(t => t.Id)
            .ToPageListAsync(pageIndex, pageSize);
    }
    /// <summary>
    ///     通过Auth获取玩家Id列表
    /// </summary>
    public async Task<Dictionary<int, (long, string)>> GetPlayerInfoListByAuth(List<int> authList)
    {
        var result = new Dictionary<int, (long, string)>();
        (await _playerRepository.Queryable()
            .Where(t => authList.Contains(t.Auth))
            .Select(t => new { t.Auth, t.Id, t.Name })
            .ToListAsync()).ForEach(t => { result.Add(t.Auth, (t.Id, t.Name)); });
        return result;
    }

    /// <summary>
    ///     更新玩家信息
    /// </summary>
    public async Task UpdateStatsNumber()
    {
        // 地图完成数
        var succeesInfo = await _playerCompleteRepository.Queryable()
            .Where(t => t.MapId != null && t.PlayerId != null && t.Type == RecordTypeEnum.Main)
            .GroupBy(t => t.PlayerId)
            .Select(t =>
                new
                {
                    t.PlayerId,
                    sum = SqlFunc.AggregateCount(t)
                }).ToListAsync();
        foreach (var item in succeesInfo)
        {
            await _playerRepository.Updateable().SetColumns(t => new PlayerModel()
            {
                SucceesNumber = item.sum
            }).Where(t => t.Id == item.PlayerId).ExecuteCommandAsync();
        }

        // 主线wr
        var wrInfo = (await _mapServices.GetMapWrList(RecordTypeEnum.Main))
            .GroupBy(t => t.PlayerId)
            .Select(t => new
            {
                PlayerId = t.Key,
                sum = t.Count()
            });
        foreach (var item in wrInfo)
        {
            await _playerRepository.Updateable().SetColumns(t => new PlayerModel()
            {
                WRNumber = item.sum
            }).Where(t => t.Id == item.PlayerId).ExecuteCommandAsync();
        }

        // 奖励wr
        var wrBountyInfo = (await _mapServices.GetMapWrList(RecordTypeEnum.Bounty))
            .GroupBy(t => t.PlayerId)
            .Select(t => new
            {
                PlayerId = t.Key,
                sum = t.Count()
            });
        foreach (var item in wrBountyInfo)
        {
            await _playerRepository.Updateable().SetColumns(t => new PlayerModel()
            {
                BWRNumber = item.sum
            }).Where(t => t.Id == item.PlayerId).ExecuteCommandAsync();
        }

        // 阶段wr
        var wrStageInfo = (await _mapServices.GetMapWrList(RecordTypeEnum.Bounty))
            .GroupBy(t => t.PlayerId)
            .Select(t => new
            {
                PlayerId = t.Key,
                sum = t.Count()
            });
        foreach (var item in wrBountyInfo)
        {
            await _playerRepository.Updateable().SetColumns(t => new PlayerModel()
            {
                SWRNumber = item.sum
            }).Where(t => t.Id == item.PlayerId).ExecuteCommandAsync();
        }
    }

    /// <summary>
    ///     修改信息
    /// </summary>
    public async Task ChangeInfo(List<PlayerModel> changeList)
    {
        _playerRepository.Updates(changeList);
        //修改冗余字段名字
        var list = await _playerCompleteRepository.Queryable()
            .Where(t => changeList.Select(a => a.Id).Contains(t.PlayerId)).ToListAsync();
        list.ForEach(t => t.PlayerName = changeList.First(a => a.Id == t.PlayerId).Name);
        _playerCompleteRepository.Updates(list);
    }

    /// <summary>
    ///     通过玩家名称获取玩家信息
    /// </summary>
    public async Task<List<PlayerModel>> GetPlayersByNames(List<string> names)
    {
        return await _playerRepository.Queryable().Where(t => names.Contains(t.Name)).ToListAsync();
    }
    /// <summary>
    ///     获取玩家未完成Count
    /// </summary>
    public async Task<int> GetPlayerFailCount(long id, RecordTypeEnum recordType, string difficulty)
    {
        // 1. 查询目标难度所有地图
        var mapsQuery = _mapRepository.Queryable()
            .Where(t => t.Difficulty == difficulty)
            .Select(t => new
            {
                t.Id,
                Number = recordType == RecordTypeEnum.Main ? 1 :
                    recordType == RecordTypeEnum.Bounty ? t.BonusNumber : t.StageNumber
            });
        // 2. 查询玩家每张地图的完成数
        var playerCompleteQuery = _playerCompleteRepository.Queryable()
            .Where(t => t.PlayerId == id && t.Type == recordType)
            .GroupBy(t => t.MapId)
            .Select(t => new { t.MapId, Number = SqlFunc.AggregateCount(t) });
        // 3. 左连接，统计未完成

        var result = await mapsQuery
            .LeftJoin(playerCompleteQuery, (map, pc) => map.Id == pc.MapId)
            .Select((map, pc) => new
            {
                map.Id,
                map.Number, // 地图要求总数
                PlayerNumber = pc.Number // 玩家完成数，没有则为0
            })
            .Select(map => new
            {
                map.Id,
                Number = map.Number - map.PlayerNumber
            })
            .CountAsync(map => map.Number != 0);
        return result;
    }
}