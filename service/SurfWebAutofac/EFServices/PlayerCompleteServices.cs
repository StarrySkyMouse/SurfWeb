using Common.Db.EFCore.Repository;
using EFServices.Base;
using IServices.Main;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Model.Dtos.Rankings;
using Model.Models.Main;

namespace EFServices;

public class PlayerCompleteServices : BaseServices<PlayerCompleteModel>, IPlayerCompleteServices
{
    private readonly IBaseRepository<MapModel> _mapRepository;
    private readonly IMapServices _mapServices;
    private readonly IBaseRepository<PlayerCompleteModel> _playerCompleteRepository;
    private readonly IBaseRepository<PlayerModel> _playerRepository;

    public PlayerCompleteServices(IBaseRepository<MapModel> mapRepository,
        IBaseRepository<PlayerModel> playerRepository,
        IBaseRepository<PlayerCompleteModel> playerCompleteRepository, IMapServices mapServices) : base(
        playerCompleteRepository)
    {
        _mapRepository = mapRepository;
        _playerRepository = playerRepository;
        _playerCompleteRepository = playerCompleteRepository;
        _mapServices = mapServices;
    }

    /// <summary>
    ///     获取主线/奖励、阶段的最后更新时间
    /// </summary>
    /// <returns></returns>
    public (DateTime?, DateTime?) GetFinallyDateTime()
    {
        var MainOrBountyDateTime = _playerCompleteRepository.IgnoreQueryFilters()
            .Where(t => t.Type == RecordTypeEnum.Main || t.Type == RecordTypeEnum.Bounty)
            .OrderByDescending(t => t.Date)
            .FirstOrDefault()?.Date;
        var StageDateTime = _playerCompleteRepository.IgnoreQueryFilters()
            .Where(t => t.Type == RecordTypeEnum.Stage)
            .OrderByDescending(t => t.Date)
            .FirstOrDefault()?.Date;
        return (MainOrBountyDateTime, StageDateTime);
    }

    /// <summary>
    ///     通过时间查询
    /// </summary>
    public async Task<List<PlayerCompleteModel>> GetByDate(DateTime date, List<RecordTypeEnum> typeList)
    {
        return await _playerCompleteRepository
            .IgnoreQueryFilters()
            .Where(t => typeList.Contains(t.Type) && t.Date >= date)
            .ToListAsync();
    }

    /// <summary>
    ///     处理增量过来的数据还未完成关联的
    /// </summary>
    public async Task DisposeDataAssociation()
    {
        var pendingData =
            await _playerCompleteRepository.Where(t => t.PlayerId == null || t.MapId == null).ToListAsync();
        var changeList = new List<PlayerCompleteModel>();
        if (pendingData.Any())
        {
            var pendingPlayerList = pendingData.Where(t => t.PlayerId == null).ToList();
            var playerDc = (await _playerRepository
                .Where(t => pendingPlayerList.Select(a => a.Auth).Contains(t.Auth))
                .ToListAsync()).ToDictionary(t => t.Auth);
            var pendingMapIdList = pendingData.Where(t => t.MapId == null).ToList();
            var mapDc = (await _mapRepository
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
            {
                foreach (var item in changeList) _playerCompleteRepository.Update(item);
                _playerCompleteRepository.SaveChanges();
            }
        }
    }

    /// <summary>
    ///     隐藏不相关的数据
    /// </summary>
    public async Task HideUnLikeData()
    {
        //设置有效地图
        var validList = await _playerCompleteRepository
            .IgnoreQueryFilters()
            .Where(t => _mapRepository.Select(a => a.Id).Contains(t.MapId) && t.IsDelete == 0)
            .ToListAsync();
        //设置无效地图
        var invalidList = await _playerCompleteRepository
            .IgnoreQueryFilters()
            .Where(t => !_mapRepository.Select(a => a.Id).Contains(t.MapId) && t.IsDelete == 0)
            .ToListAsync();
        validList.ForEach(t => t.Hide = false);
        invalidList.ForEach(t => t.Hide = true);
        _playerCompleteRepository.Updates(validList);
        _playerCompleteRepository.Updates(invalidList);
        _playerCompleteRepository.SaveChanges();
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
            var predicate = PredicateBuilder.New<PlayerCompleteModel>(false);
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
            var subResult = await _playerCompleteRepository
                .IgnoreQueryFilters().Where(predicate).ToListAsync();
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
            var predicate = PredicateBuilder.New<PlayerCompleteModel>(false);
            // 处理每一批 batch
            foreach (var item in batch)
                predicate = predicate.Or(t =>
                    t.Auth == item.auth &&
                    t.MapName == item.map &&
                    t.Type == RecordTypeEnum.Stage &&
                    t.Stage == item.stage
                );
            var subResult = await _playerCompleteRepository
                .IgnoreQueryFilters().Where(predicate).ToListAsync();
            result.AddRange(subResult);
        }

        return result;
    }

    public async Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType)
    {
        var result = new List<RankingDto>();
        if (rankingType == RankingTypeEnum.Integral)
        {
            result = (await _playerRepository.OrderByDescending(t => t.Integral)
                .Take(10)
                .ToListAsync()).Select(t => new RankingDto
            {
                Type = rankingType,
                PlayerId = t.Id,
                PlayerName = t.Name,
                Value = t.Integral
            }).ToList();
            var mapMainList = await _mapServices.GetMapMainList();
            var playerIds = result.Select(a => a.PlayerId).ToList();
            var succeedInfo = await _playerCompleteRepository
                .Where(t => playerIds.Contains(t.PlayerId) && t.Type == RecordTypeEnum.Main)
                .GroupBy(t => t.PlayerId)
                .Select(g => new
                {
                    PlayerId = g.Key,
                    Sum = g.Count()
                }).ToListAsync();

            result.ForEach(t =>
            {
                t.Progress =
                    $"{succeedInfo.FirstOrDefault(a => a.PlayerId == t.PlayerId)?.Sum ?? 0}/{mapMainList.Count}";
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