using IServices.Main;
using Model.Dtos.Players;
using Model.Models.Main;
using Repository.BASE.MainSqlSugar;
using Services.Base;
using SqlSugar;

namespace Services.Main;

public class PlayerServices : BaseServices<PlayerModel>, IPlayerServices
{
    private readonly IMainRepository<MapModel> _mapRepository;
    private readonly IMainRepository<PlayerCompleteModel> _playerCompleteRepository;
    private readonly IMainRepository<PlayerModel> _playerRepository;

    public PlayerServices(IMainRepository<PlayerModel> playerRepository,
        IMainRepository<PlayerCompleteModel> playerCompleteRepository,
        IMainRepository<MapModel> mapRepository)
    {
        _playerRepository = playerRepository;
        _playerCompleteRepository = playerCompleteRepository;
        _mapRepository = mapRepository;
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
    /// <summary>
    /// 获取玩家WRCount
    /// </summary>
    //public int GetPlayerWRCount(string id, RecordTypeEnum recordType, string difficulty)
    //{
    //    return GetPlayerWRQueryable(id, recordType, difficulty).Count();
    //}

    /// <summary>
    /// 获取玩家WRList
    /// </summary>
    //public List<PlayerWRDto> GetPlayerWRList(string id, RecordTypeEnum recordType, string difficulty, int pageIndex)
    //{
    //    return GetPlayerWRQueryable(id, recordType, difficulty)
    //        .OrderByDescending(t => t.Key)
    //        .PageData(pageIndex, 10)
    //        .Select(t => new PlayerWRDto()
    //        {
    //            MapId = t.Key,
    //            MapName = t.First().MapName,
    //            Difficulty = t.First().Difficulty,
    //            Img = t.First().Img,
    //            Stages = t.Select(a => new PlayerStageDto()
    //            {
    //                Stage = a.Stage,
    //                Time = a.Time,
    //                Date = a.Date
    //            }).OrderBy(a => a.Stage).ToList()
    //        }).ToList();
    //}
    //private IEnumerable<IGrouping<string, MapWrCache>> GetPlayerWRQueryable(string id, RecordTypeEnum recordType, string difficulty)
    //{
    //    return _dataCache.MapSnapshot.MapWrCaches
    //        .OrderByDescending(t => t.Date)
    //        .Where(t => t.PlayerId == id && t.Type == recordType && t.Difficulty == difficulty)
    //        .GroupBy(t => t.MapId);
    //}
    /// <summary>
    ///     获取玩家已完成Count
    /// </summary>
    public async Task<int> GetPlayerSucceesCount(string id, RecordTypeEnum recordType, string difficulty)
    {
        return await GetPlayerSucceesQueryable(id, recordType, difficulty).CountAsync();
    }

    /// <summary>
    ///     获取玩家已完成List
    /// </summary>
    public async Task<List<PlayerSucceesDto>> GetPlayerSucceesList(string id, RecordTypeEnum recordType,
        string difficulty, int pageIndex)
    {
        // 1. 查询玩家通关记录
        var queryKeyList = (await GetPlayerSucceesQueryable(id, recordType, difficulty)
            .OrderByDescending(t => t.Key)
            .Select(t => t.Select(a => a.Id).ToList())
            .ToPageListAsync(pageIndex, 10)).SelectMany(t => t);
        var queryList = (await _playerCompleteRepository.Queryable()
                .Where(t => queryKeyList.Contains(t.Id) && t.Type == recordType)
                .ToListAsync())
            .OrderByDescending(t => t.Date)
            .GroupBy(t => t.MapId);
        // 2. 关联Map表，查询地图难度信息
        var mapInfoList =
            await _mapRepository.Queryable().Where(t => queryList.Select(a => a.Key).Contains(t.Id)).ToListAsync();
        //3.查询wr信息
        //var mapWrList = _dataCache.MapSnapshot.MapWrCaches.Where(t =>
        //    queryList.Select(a => a.Key).Contains(t.MapId) && t.Type == recordType);
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
                //GapTime = mapWrList.Any(b => b.MapId == t.Key && b.Stage == a.Stage) ? (mapWrList.FirstOrDefault(b => b.MapId == t.Key && b.Stage == a.Stage).Time - a.Time) : -1,
                Date = a.Date
            }).OrderBy(a => a.Stage).ToList()
        }).ToList();
        return result;
    }

    /// <summary>
    ///     获取玩家未完成List
    /// </summary>
    public async Task<List<PlayerFailDto>> GetPlayerFailList(string id, RecordTypeEnum recordType, string difficulty,
        int pageIndex)
    {
        //if (recordType == RecordTypeEnum.Main)
        //{
        //    var list = await _playerCompleteRepository
        //            .Join(_mapRepository.Where(t => t.Difficulty == difficulty), a => a.MapId, b => b.Id, (a, b) => a)
        //            .Where(a => a.PlayerId == id && a.Type == recordType)
        //            .Select(a => a.MapId).ToListAsync();
        //    return _dataCache.MapSnapshot.MapMainList
        //        .Where(t => !list.Contains(t.Id) && t.Difficulty == difficulty)
        //        .OrderBy(t => t.Name)
        //        .Skip((pageIndex - 1) * 10)
        //        .Take(10)
        //        .Select(t => new PlayerFailDto()
        //        {
        //            MapId = t.Id,
        //            MapName = t.Name,
        //            Difficulty = t.Difficulty,
        //            Img = t.Img
        //        }).ToList();
        //}
        //else if (recordType == RecordTypeEnum.Bounty)
        //{
        //    var list = await _playerCompleteRepository
        //                .Join(_mapRepository.Where(t => t.Difficulty == difficulty), a => a.MapId, b => b.Id, (a, b) => a)
        //                .Where(a => a.PlayerId == id && a.Type == recordType)
        //                .Select(a => a.MapId + "@@" + a.Stage).ToListAsync();
        //    return _dataCache.MapSnapshot.MapBountyList
        //        //获取未完成的阶段
        //        .Where(t => !list.Contains(t.Id + "@@" + t.Stage) && t.Difficulty == difficulty)
        //        .GroupBy(t => t.Id)
        //        .OrderBy(t => t.Key)
        //        .PageData(pageIndex, 10)
        //        .Select(t => new PlayerFailDto()
        //        {
        //            MapId = t.Key,
        //            MapName = t.First().Name,
        //            Difficulty = t.First().Difficulty,
        //            Img = t.First().Img,
        //            Stages = t.Select(a => a.Stage).ToList()
        //        }).ToList();
        //}
        //else if (recordType == RecordTypeEnum.Stage)
        //{
        //    var list = await _playerCompleteRepository
        //        .Join(_mapRepository.Where(t => t.Difficulty == difficulty), a => a.MapId, b => b.Id, (a, b) => a)
        //        .Where(a => a.PlayerId == id && a.Type == recordType)
        //        .Select(a => a.MapId + "@@" + a.Stage).ToListAsync();
        //    return _dataCache.MapSnapshot.MapStageList
        //        //获取未完成的阶段
        //        .Where(t => !list.Contains(t.Id + "@@" + t.Stage) && t.Difficulty == difficulty)
        //        .GroupBy(t => t.Id)
        //        .OrderBy(t => t.Key)
        //        .PageData(pageIndex, 10)
        //        .Select(t => new PlayerFailDto()
        //        {
        //            MapId = t.Key,
        //            MapName = t.First().Name,
        //            Difficulty = t.First().Difficulty,
        //            Img = t.First().Img,
        //            Stages = t.Select(a => a.Stage).ToList()
        //        }).ToList();
        //}

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
        //// 地图完成数
        //var succeesInfo = await _playerCompleteRepository.Queryable()
        //     .Where(t => t.MapId != null && t.PlayerId != null && t.Type == RecordTypeEnum.Main)
        //     .GroupBy(t => t.PlayerId)
        //     .Select(t =>
        //     new
        //     {
        //         PlayerId = t.Key,
        //         sum = t.Count()
        //     }).ToListAsync();
        //// 主线wr
        //var wrInfo = await _playerCompleteRepository
        //      .Where(t => t.MapId != null && t.PlayerId != null && t.Type == RecordTypeEnum.Main)
        //      .GroupBy(t => t.MapId)
        //      .Select(t => new
        //      {
        //          MapId = t.Key,
        //          WR = t.OrderBy(p => p.Time).First(),
        //      }).ToListAsync();
        //// 奖励wr
        //var wrBountyInfo = await _playerCompleteRepository
        //    .Where(t => t.MapId != null && t.PlayerId != null && t.Type == RecordTypeEnum.Bounty)
        //    .GroupBy(t => new
        //    {
        //        t.MapId,
        //        t.Stage
        //    })
        //    .Select(t => new
        //    {
        //        MapId = t.Key,
        //        WR = t.OrderBy(p => p.Time).First(),
        //    }).ToListAsync();
        //// 阶段wr
        //var wrStageInfo = await _playerCompleteRepository
        //    .Where(t => t.MapId != null && t.PlayerId != null && t.Type == RecordTypeEnum.Stage)
        //    .GroupBy(t => new
        //    {
        //        t.MapId,
        //        t.Stage
        //    })
        //    .Select(t => new
        //    {
        //        MapId = t.Key,
        //        WR = t.OrderBy(p => p.Time).First(),
        //    }).ToListAsync();
        //var playerSucceesIdList = new List<string>();
        //playerSucceesIdList.AddRange(succeesInfo.Select(t => t.PlayerId));
        //playerSucceesIdList.AddRange(wrInfo.Select(t => t.WR.PlayerId));
        //playerSucceesIdList.AddRange(wrBountyInfo.Select(t => t.WR.PlayerId));
        //playerSucceesIdList.AddRange(wrStageInfo.Select(t => t.WR.PlayerId));
        //playerSucceesIdList = playerSucceesIdList.Distinct().ToList();
        //var playerSucceesList = await _playerRepository
        //    .Where(t => playerSucceesIdList.Contains(t.Id))
        //    .ToListAsync();
        //foreach (var item in playerSucceesList)
        //{
        //    item.SucceesNumber = succeesInfo.FirstOrDefault(t => t.PlayerId == item.Id)?.sum ?? 0;
        //    item.WRNumber = wrInfo.Count(t => t.WR.PlayerId == item.Id);
        //    item.BWRNumber = wrBountyInfo.Count(t => t.WR.PlayerId == item.Id);
        //    item.SWRNumber = wrStageInfo.Count(t => t.WR.PlayerId == item.Id);
        //    _playerRepository.Update(item);
        //}
        //_playerRepository.SaveChanges();
        throw new AbandonedMutexException();
    }

    /// <summary>
    ///     修改信息
    /// </summary>
    public async Task ChangeInfo(List<PlayerModel> changeList)
    {
        //_playerRepository.Updateable(changeList);
        ////修改冗余字段名字
        //var list = await _playerCompleteRepository.Where(t => changeList.Select(a => a.Id).Contains(t.PlayerId)).ToListAsync();
        //list.ForEach(t => t.PlayerName = changeList.First(a => a.Id == t.PlayerId).Name);
        //_playerCompleteRepository.Updates(list);
        //_playerRepository.SaveChanges();
    }

    private ISugarQueryable<IGrouping<string?, PlayerCompleteModel>> GetPlayerSucceesQueryable(string id,
        RecordTypeEnum recordType, string difficulty)
    {
        //return _playerCompleteRepository.Queryable()
        //    .InnerJoin(_mapRepository.Queryable().Where(t => t.Difficulty == difficulty), a => a.MapId, b => b.Id, (a, b) => a)
        //    .OrderByDescending(t => t.Date)
        //    .Where(t => t.PlayerId == id && t.Type == recordType)
        //    .GroupBy(t => t.MapId);
        throw new AbandonedMutexException();
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

        //// 2. 查询玩家每张地图的完成数
        //var playerCompleteQuery = _playerCompleteRepository.Queryable()
        //    .Where(t => t.PlayerId == id && t.Type == recordType)
        //    .GroupBy(t => t.MapId)
        //    .Select(g => new { MapId = g.Key, Number = g.Count() });

        //// 3. 左连接，统计未完成
        //var result = await mapsQuery
        //    .GroupJoin(
        //        playerCompleteQuery,
        //        map => map.Id,
        //        pc => pc.MapId,
        //        (map, pcs) => new
        //        {
        //            map.Id,
        //            map.Number, // 地图要求总数
        //            PlayerNumber = pcs.Select(x => x.Number).FirstOrDefault() // 玩家完成数，没有则为0
        //        }
        //    )
        //    .Select(x => new
        //    {
        //        x.Id,
        //        Number = x.Number - x.PlayerNumber
        //    })
        //    .CountAsync(x => x.Number != 0);
        //return result;
        throw new AbandonedMutexException();
    }
}