using Common.Caches.AOP;
using Common.Db.SqlSugar.Repository.Main;
using IServices.Main;
using Model.Dtos.Rankings;
using Model.Models.Main;
using Services.Base;
using Services.Main.ExtensionsRepository;
using SqlSugar;

namespace Services.Main;

//设置缓存2分钟
[Cache(CacheTime = 120)]
public class PlayerCompleteServices : BaseServices<PlayerCompleteModel>, IPlayerCompleteServices
{
    private readonly IMainRepository<MapModel> _mapRepository;
    private readonly IMapServices _mapServices;
    private readonly PlayerCompleteRepository _playerCompleteRepository;
    private readonly IMainRepository<PlayerModel> _playerRepository;

    public PlayerCompleteServices(PlayerCompleteRepository playerCompleteRepository,
        IMainRepository<PlayerModel> playerRepository,
        IMainRepository<MapModel> mapRepository, IMapServices mapServices) : base(playerCompleteRepository)
    {
        _playerCompleteRepository = playerCompleteRepository;
        _playerRepository = playerRepository;
        _mapRepository = mapRepository;
        _mapServices = mapServices;
    }

    /// <summary>
    ///     获取主线/奖励、阶段的最后更新时间
    /// </summary>
    /// <returns></returns>
    public (DateTime?, DateTime?) GetFinallyDateTime()
    {
        var mainOrBountyDateTime = _playerCompleteRepository.QueryableNoHide()
            .Where(t => t.Type == RecordTypeEnum.Main || t.Type == RecordTypeEnum.Bounty)
            .OrderByDescending(t => t.Date)
            .First()?.Date;
        var stageDateTime = _playerCompleteRepository.QueryableNoHide()
            .Where(t => t.Type == RecordTypeEnum.Stage)
            .OrderByDescending(t => t.Date)
            .First()?.Date;
        return (mainOrBountyDateTime, stageDateTime);
    }

    /// <summary>
    ///     通过时间查询
    /// </summary>
    public async Task<List<PlayerCompleteModel>> GetByDate(DateTime date, List<RecordTypeEnum> typeList)
    {
        return await _playerCompleteRepository.QueryableNoHide()
            .Where(t => typeList.Contains(t.Type) && t.Date >= date)
            .ToListAsync();
    }

    /// <summary>
    ///     处理增量过来的数据还未完成关联的
    /// </summary>
    public async Task DisposeDataAssociation()
    {
        var pendingData = await _playerCompleteRepository.Queryable().Where(t => t.PlayerId == null || t.MapId == null)
            .ToListAsync();
        var changeList = new List<PlayerCompleteModel>();
        if (pendingData.Any())
        {
            var pendingPlayerList = pendingData.Where(t => t.PlayerId == null).ToList();
            var playerDc = (await _playerRepository.Queryable()
                .Where(t => pendingPlayerList.Select(a => a.Auth).Contains(t.Auth))
                .ToListAsync()).ToDictionary(t => t.Auth);
            var pendingMapIdList = pendingData.Where(t => t.MapId == null).ToList();
            var mapDc = (await _mapRepository.Queryable()
                .Where(t => pendingMapIdList.Select(a => a.MapName).Contains(t.Name))
                .ToListAsync()).ToDictionary(t => t.Name);
            var flag = false;
            foreach (var item in pendingData)
            {
                if (playerDc.TryGetValue(item.Auth, out var player))
                {
                    item.PlayerId = player.Id;
                    item.PlayerName = player.Name;
                    flag = true;
                }

                if (mapDc.TryGetValue(item.MapName, out var map))
                {
                    item.MapId = map.Id;
                    flag = true;
                }

                if (flag) changeList.Add(item);
                flag = false;
            }

            if (changeList.Any())
                foreach (var item in changeList)
                    _playerCompleteRepository.Update(item);
        }
    }

    /// <summary>
    ///     隐藏不相关的数据
    /// </summary>
    public async Task HideUnLikeData()
    {
        ////设置有效地图
        var validList = await _playerCompleteRepository.QueryableNoHide()
            .Where(t => SqlFunc.Subqueryable<MapModel>().Where(a => a.Id == t.MapId).Any())
            .ToListAsync();
        //设置无效地图
        var invalidList = await _playerCompleteRepository.QueryableNoHide()
            .Where(t => SqlFunc.Subqueryable<MapModel>().Where(a => a.Id == t.MapId).NotAny())
            .ToListAsync();
        validList.ForEach(t => t.Hide = false);
        invalidList.ForEach(t => t.Hide = true);
        _playerCompleteRepository.Updates(validList);
        _playerCompleteRepository.Updates(invalidList);
    }

    /// <summary>
    ///     获取旧的数据
    /// </summary>
    public async Task<List<PlayerCompleteModel>> GetOldPlayertimesData(
        IEnumerable<(int auth, string map, int track)> list)
    {
        var result = new List<PlayerCompleteModel>();
        var batchSize = 100;
        var total = list.Count();
        for (var i = 0; i < total; i += batchSize)
        {
            var batch = list.Skip(i).Take(batchSize).ToList();
            var predicate = Expressionable.Create<PlayerCompleteModel>();
            // 处理每一批 batch
            foreach (var item in batch)
                predicate = predicate.Or(t =>
                    t.Auth == item.auth &&
                    t.MapName == item.map &&
                    (
                        (item.track == 0 && t.Type == RecordTypeEnum.Main) ||
                        (item.track != 0 && t.Type == RecordTypeEnum.Bounty)
                    ) &&
                    (
                        (item.track == 0 && t.Stage == null) ||
                        (item.track != 0 && t.Stage == item.track)
                    )
                );
            var subResult = await _playerCompleteRepository.QueryableNoHide()
                .Where(predicate.ToExpression()).ToListAsync();
            result.AddRange(subResult);
        }

        return result;
    }

    /// <summary>
    ///     获取旧的数据(阶段)
    /// </summary>
    public async Task<List<PlayerCompleteModel>> GetOldStagetimesData(
        IEnumerable<(int auth, string map, int stage)> list)
    {
        var result = new List<PlayerCompleteModel>();
        var batchSize = 100;
        var total = list.Count();
        for (var i = 0; i < total; i += batchSize)
        {
            var batch = list.Skip(i).Take(batchSize).ToList();
            var predicate = Expressionable.Create<PlayerCompleteModel>();
            // 处理每一批 batch
            foreach (var item in batch)
                predicate = predicate.Or(t =>
                    t.Auth == item.auth &&
                    t.MapName == item.map &&
                    t.Type == RecordTypeEnum.Stage &&
                    t.Stage == item.stage
                );

            var subResult = await _playerCompleteRepository.QueryableNoHide()
                .Where(predicate.ToExpression()).ToListAsync();
            result.AddRange(subResult);
        }

        return result;
    }

    /// <summary>
    ///     获取排名
    /// </summary>
    [NoCache]
    public async Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType)
    {
        var result = new List<RankingDto>();
        if (rankingType == RankingTypeEnum.Integral)
        {
            result = (await _playerRepository.Queryable().OrderByDescending(t => t.Integral)
                .Take(10)
                .ToListAsync()).Select(t => new RankingDto
            {
                Type = rankingType,
                PlayerId = t.Id,
                PlayerName = t.Name,
                Value = t.Integral
            }).ToList();
            var mapMainList = await _mapServices.GetMapMainList();
            var succeedInfo = await _playerCompleteRepository.Queryable()
                .Where(t => result.Select(a => a.PlayerId).Contains(t.PlayerId) && t.Type == RecordTypeEnum.Main)
                .GroupBy(t => t.PlayerId)
                .Select(t => new
                {
                    t.PlayerId,
                    Sum = SqlFunc.AggregateCount(t)
                }).ToListAsync();
            result.ForEach(t =>
            {
                t.Progress =
                    $"{succeedInfo.FirstOrDefault(a => a.PlayerId == t.PlayerId)?.Sum}/{mapMainList.Count}";
            });
        }
        else
        {
            var wrList = await _mapServices.GetMapWrList((RecordTypeEnum)rankingType - 1);
            result = wrList.GroupBy(t => t.PlayerId)
                .Select(g => new RankingDto
                {
                    Type = rankingType,
                    PlayerId = g.First().PlayerId,
                    PlayerName = g.First().PlayerName,
                    Value = g.Count()
                }).OrderByDescending(t => t.Value).Take(10).ToList();
        }

        return result;
    }
}