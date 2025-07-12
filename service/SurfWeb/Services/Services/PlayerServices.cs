using Microsoft.EntityFrameworkCore;
using Model.Cahces;
using Model.Dtos.Players;
using Model.Entitys;
using Repositories.IRepository;
using Services.Base;
using Services.IServices;
using Utils.Extensions;

namespace Services.Services
{
    public class PlayerServices : BaseServices<PlayerModel>, IPlayerServices
    {
        private readonly IPlayerRepository _repository;
        private readonly IPlayerCompleteRepository _playerCompleteRepository;
        private readonly IMapRepository _mapRepository;
        private readonly DataCache _dataCache;
        public PlayerServices(IPlayerRepository repository,
            IPlayerCompleteRepository playerCompleteRepository,
            IMapRepository mapRepository,
            DataCache dataCache) : base(repository)
        {
            _repository = repository;
            _playerCompleteRepository = playerCompleteRepository;
            _mapRepository = mapRepository;
            _dataCache = dataCache;
        }
        /// <summary>
        /// 获取玩家信息
        /// </summary>
        public async Task<PlayerInfoDto?> GetPlayerInfo(string id)
        {
            var result = await _repository.Select(t => new PlayerInfoDto()
            {
                Id = t.Id,
                Name = t.Name,
                Integral = t.Integral,
                SucceesNumber = t.SucceesNumber,
                WRNumber = t.WRNumber,
                BWRNumber = t.BWRNumber,
                SWRNumber = t.SWRNumber
            }).FirstOrDefaultAsync(t => t.Id == id);
            if (result != null)
            {
                //积分排行
                result.IntegralRanking = await _repository
                   .Where(t => t.Integral > result.Integral)
                   .CountAsync() + 1;
                //地图完成数排行
                result.SucceesRanking = await _repository
                   .Where(t => t.SucceesNumber > result.SucceesNumber)
                   .CountAsync() + 1;
                //主线排行
                result.WRRanking = await _repository
                   .Where(t => t.WRNumber > result.WRNumber)
                   .CountAsync() + 1;
                //奖励排行
                result.BWRanking = await _repository
                   .Where(t => t.BWRNumber > result.BWRNumber)
                   .CountAsync() + 1;
                //阶段排行
                result.SWRanking = await _repository
                   .Where(t => t.SWRNumber > result.SWRNumber)
                   .CountAsync() + 1;

            }
            return result;
        }
        /// <summary>
        /// 获取玩家WRCount
        /// </summary>
        public int GetPlayerWRCount(string id, RecordTypeEnum recordType)
        {
            return GetPlayerWRQueryable(id, recordType).Count();
        }
        /// <summary>
        /// 获取玩家WRList
        /// </summary>
        public List<PlayerWRDto> GetPlayerWRList(string id, RecordTypeEnum recordType, int pageIndex)
        {
            return GetPlayerWRQueryable(id, recordType)
                .PageData(pageIndex, 10)
                .OrderByDescending(t => t.Date)
                .Select(t => new PlayerWRDto()
                {
                    MapId = t.MapId,
                    MapName = t.MapName,
                    Difficulty = t.Difficulty,
                    Stage = t.Stage,
                    Time = t.Time,
                    Date = t.Date
                })
                .ToList();
        }
        private IEnumerable<MapWrCache> GetPlayerWRQueryable(string id, RecordTypeEnum recordType)
        {
            return _dataCache.MapSnapshot.MapWrCaches.OrderByDescending(t => t.Date).Where(t => t.PlayerId == id && t.Type == recordType);
        }
        /// <summary>
        /// 获取玩家已完成Count
        /// </summary>
        public async Task<int> GetPlayerSucceesCount(string id, RecordTypeEnum recordType)
        {
            return await GetPlayerSucceesQueryable(id, recordType).CountAsync();
        }
        /// <summary>
        /// 获取玩家已完成List
        /// </summary>
        public async Task<List<PlayerSucceesDto>> GetPlayerSucceesList(string id, RecordTypeEnum recordType, int pageIndex)
        {
            // 1. 查询玩家通关记录
            var query = GetPlayerSucceesQueryable(id, recordType)
                .PageData(pageIndex, 10)
                .OrderByDescending(t => t.Date);
            // 2. 关联Map表，查询地图难度信息
            var result = await query
                .Join(_mapRepository,
                    player => player.MapId,
                    map => map.Id,
                    (player, map) => new
                    {
                        player.MapId,
                        player.MapName,
                        player.Stage,
                        player.Time,
                        player.Date,
                        map.Difficulty
                    })
                .Select(t => new PlayerSucceesDto()
                {
                    MapId = t.MapId,
                    MapName = t.MapName,
                    Stage = t.Stage,
                    Time = t.Time,
                    Date = t.Date,
                    Difficulty = t.Difficulty
                }).ToListAsync();
            return result;
        }
        private IQueryable<PlayerCompleteModel> GetPlayerSucceesQueryable(string id, RecordTypeEnum recordType)
        {
            return _playerCompleteRepository
                .OrderByDescending(t => t.Date)
                .Where(t => t.PlayerId == id && t.Type == recordType);
        }
        /// <summary>
        /// 获取玩家未完成Count
        /// </summary>
        public async Task<int> GetPlayerFailCount(string id, RecordTypeEnum recordType)
        {
            var sum = 0;
            switch (recordType)
            {
                case RecordTypeEnum.Main:
                    sum = await _mapRepository.CountAsync();
                    break;
                case RecordTypeEnum.Bounty:
                    sum = await _mapRepository.Select(t => t.BonusNumber).SumAsync();
                    break;
                case RecordTypeEnum.Stage:
                    sum = await _mapRepository.Select(t => t.StageNumber).SumAsync();
                    break;
            }
            return sum - await _playerCompleteRepository.CountAsync(t => t.PlayerId == id && t.Type == recordType);
        }
        /// <summary>
        /// 获取玩家未完成List
        /// </summary>
        public async Task<List<PlayerFailDto>> GetPlayerFailList(string id, RecordTypeEnum recordType, int pageIndex)
        {
            if (recordType == RecordTypeEnum.Main)
            {
                var list = await _playerCompleteRepository
                        .Where(a => a.PlayerId == id && a.Type == recordType)
                        .Select(a => a.MapId).ToListAsync();
                return _dataCache.MapSnapshot.MapMainList
                    .Where(t => !list.Contains(t.Id))
                    .OrderBy(t => t.Name)
                    .Skip((pageIndex - 1) * 10)
                    .Take(10)
                    .Select(t => new PlayerFailDto()
                    {
                        MapId = t.Id,
                        MapName = t.Name,
                        Difficulty = t.Difficulty,
                    }).ToList();
            }
            else if (recordType == RecordTypeEnum.Bounty)
            {
                var list = await _playerCompleteRepository
                            .Where(a => a.PlayerId == id && a.Type == recordType)
                            .Select(a => a.MapId + "@@" + a.Stage).ToListAsync();
                return _dataCache.MapSnapshot.MapBountyList
                    //获取未完成的阶段
                    .Where(t => !list.Contains(t.Id + "@@" + t.Stage))
                    .OrderBy(t => t.Name)
                    .Skip((pageIndex - 1) * 10)
                    .Take(10)
                    .Select(t => new PlayerFailDto()
                    {
                        MapId = t.Id,
                        MapName = t.Name,
                        Difficulty = t.Difficulty,
                        Stage = t.Stage
                    }).ToList();
            }
            else if (recordType == RecordTypeEnum.Stage)
            {
                var list = await _playerCompleteRepository
                            .Where(a => a.PlayerId == id && a.Type == recordType)
                            .Select(a => a.MapId + "@@" + a.Stage).ToListAsync();
                return _dataCache.MapSnapshot.MapStageList
                    //获取未完成的阶段
                    .Where(t => !list.Contains(t.Id + "@@" + t.Stage))
                    .OrderBy(t => t.Name)
                    .Skip((pageIndex - 1) * 10)
                    .Take(10)
                    .Select(t => new PlayerFailDto()
                    {
                        MapId = t.Id,
                        MapName = t.Name,
                        Difficulty = t.Difficulty,
                        Stage = t.Stage
                    }).ToList();
            }
            return await Task.FromResult(new List<PlayerFailDto>());
        }
        //分页查询玩家数据
        public async Task<List<PlayerModel>> GetPlayerPageList(int pageIndex, int pageSize)
        {
            return await _repository
                .OrderByDescending(t => t.Id)
                .PageData(pageIndex, pageSize)
                .ToListAsync();
        }
        /// <summary>
        /// 通过Auth获取玩家Id列表
        /// </summary>
        public async Task<Dictionary<int, (string, string)>> GetPlayerInfoListByAuth(List<int> authList)
        {
            var result = new Dictionary<int, (string, string)>();
            (await _repository
                .Where(t => authList.Contains(t.Auth))
                .Select(t => new { t.Auth, t.Id, t.Name })
                .ToListAsync()).ForEach(t =>
                {
                    result.Add(t.Auth, (t.Id, t.Name));
                });
            return result;
        }
        /// <summary>
        /// 更新玩家信息
        /// </summary>
        public async Task UpdateStatsNumber()
        {
            // 地图完成数
            var succeesInfo = await _playerCompleteRepository
                 .Where(t => t.MapId != null && t.PlayerId != null && t.Type == RecordTypeEnum.Main)
                 .GroupBy(t => t.PlayerId)
                 .Select(t =>
                 new
                 {
                     PlayerId = t.Key,
                     sum = t.Count()
                 }).ToListAsync();
            // 主线wr
            var wrInfo = await _playerCompleteRepository
                  .Where(t => t.MapId != null && t.PlayerId != null && t.Type == RecordTypeEnum.Main)
                  .GroupBy(t => t.MapId)
                  .Select(t => new
                  {
                      MapId = t.Key,
                      WR = t.OrderBy(p => p.Time).First(),
                  }).ToListAsync();
            // 奖励wr
            var wrBountyInfo = await _playerCompleteRepository
                .Where(t => t.MapId != null && t.PlayerId != null && t.Type == RecordTypeEnum.Bounty)
                .GroupBy(t => new
                {
                    t.MapId,
                    t.Stage
                })
                .Select(t => new
                {
                    MapId = t.Key,
                    WR = t.OrderBy(p => p.Time).First(),
                }).ToListAsync();
            // 阶段wr
            var wrStageInfo = await _playerCompleteRepository
                .Where(t => t.MapId != null && t.PlayerId != null && t.Type == RecordTypeEnum.Stage)
                .GroupBy(t => new
                {
                    t.MapId,
                    t.Stage
                })
                .Select(t => new
                {
                    MapId = t.Key,
                    WR = t.OrderBy(p => p.Time).First(),
                }).ToListAsync();
            var playerSucceesIdList = new List<string>();
            playerSucceesIdList.AddRange(succeesInfo.Select(t => t.PlayerId));
            playerSucceesIdList.AddRange(wrInfo.Select(t => t.WR.PlayerId));
            playerSucceesIdList.AddRange(wrBountyInfo.Select(t => t.WR.PlayerId));
            playerSucceesIdList.AddRange(wrStageInfo.Select(t => t.WR.PlayerId));
            playerSucceesIdList = playerSucceesIdList.Distinct().ToList();
            var playerSucceesList = await _repository
                .Where(t => playerSucceesIdList.Contains(t.Id))
                .ToListAsync();
            foreach (var item in playerSucceesList)
            {
                item.SucceesNumber = succeesInfo.FirstOrDefault(t => t.PlayerId == item.Id)?.sum ?? 0;
                item.WRNumber = wrInfo.Count(t => t.WR.PlayerId == item.Id);
                item.BWRNumber = wrBountyInfo.Count(t => t.WR.PlayerId == item.Id);
                item.SWRNumber = wrStageInfo.Count(t => t.WR.PlayerId == item.Id);
                _repository.Update(item);
            }
            _repository.SaveChanges();
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        public async Task ChangeInfo(List<PlayerModel> changeList)
        {
            _repository.Updates(changeList);
            //修改冗余字段名字
            var list = await _playerCompleteRepository.Where(t => changeList.Select(a => a.Id).Contains(t.PlayerId)).ToListAsync();
            list.ForEach(t => t.PlayerName = changeList.First(a => a.Id == t.PlayerId).Name);
            _playerCompleteRepository.Updates(list);
            _repository.SaveChanges();
        }
    }
}
