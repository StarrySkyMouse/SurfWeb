using LinqKit;
using Microsoft.EntityFrameworkCore;
using Model.Entitys;
using Repositories.IRepository;
using Services.Base;
using Services.IServices;

namespace Services.Services
{
    public class PlayerCompleteServices : BaseServices<PlayerCompleteModel>, IPlayerCompleteServices
    {
        private readonly IPlayerCompleteRepository _repository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapRepository _mapRepository;
        public PlayerCompleteServices(IPlayerCompleteRepository repository,
            IPlayerRepository playerRepository,
            IMapRepository mapRepository) : base(repository)
        {
            _repository = repository;
            _playerRepository = playerRepository;
            _mapRepository = mapRepository;
        }
        /// <summary>
        /// 获取主线/奖励、阶段的最后更新时间
        /// </summary>
        /// <returns></returns>
        public (DateTime?, DateTime?) GetFinallyDateTime()
        {
            var MainOrBountyDateTime = _repository.IgnoreQueryFilters()
                .Where(t => t.Type == RecordTypeEnum.Main || t.Type == RecordTypeEnum.Bounty)
                .OrderByDescending(t => t.Date)
                .FirstOrDefault()?.Date;
            var StageDateTime = _repository.IgnoreQueryFilters()
                .Where(t => t.Type == RecordTypeEnum.Stage)
                .OrderByDescending(t => t.Date)
                .FirstOrDefault()?.Date;
            return (MainOrBountyDateTime, StageDateTime);
        }
        /// <summary>
        /// 通过时间查询
        /// </summary>
        public async Task<List<PlayerCompleteModel>> GetByDate(DateTime date, List<RecordTypeEnum> typeList)
        {
            return await _repository
                .IgnoreQueryFilters()
                .Where(t => typeList.Contains(t.Type) && t.Date >= date)
                .ToListAsync();
        }
        /// <summary>
        /// 处理增量过来的数据还未完成关联的
        /// </summary>
        public async Task DisposeDataAssociation()
        {
            var pendingData = await _repository.Where(t => t.PlayerId == null || t.MapId == null).ToListAsync();
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
                    if (flag)
                    {
                        changeList.Add(item);
                    }
                    flag = false;
                }
                if (changeList.Any())
                {
                    foreach (var item in changeList)
                    {
                        _repository.Update(item);
                    }
                    _repository.SaveChanges();
                }
            }
        }
        /// <summary>
        /// 隐藏不相关的数据
        /// </summary>
        public async Task HideUnLikeData()
        {
            //设置有效地图
            var validList = await _repository
                .IgnoreQueryFilters()
                .Where(t => _mapRepository.Select(a => a.Id).Contains(t.MapId) && t.IsDelete == 0)
                .ToListAsync();
            //设置无效地图
            var invalidList = await _repository
                 .IgnoreQueryFilters()
                 .Where(t => !_mapRepository.Select(a => a.Id).Contains(t.MapId) && t.IsDelete == 0)
                .ToListAsync();
            validList.ForEach(t => t.Hide = false);
            invalidList.ForEach(t => t.Hide = true);
            _repository.Updates(validList);
            _repository.Updates(invalidList);
            _repository.SaveChanges();
        }

        /// <summary>
        /// 获取旧的数据
        /// </summary>
        public async Task<List<PlayerCompleteModel>> GetOldPlayertimesData(IEnumerable<(int auth, string map, int track)> list)
        {
            var result = new List<PlayerCompleteModel>();
            int batchSize = 100;
            int total = list.Count();
            for (int i = 0; i < total; i += batchSize)
            {
                var batch = list.Skip(i).Take(batchSize).ToList();
                var predicate = PredicateBuilder.New<PlayerCompleteModel>(false);
                // 处理每一批 batch
                foreach (var item in batch)
                {
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
                }
                var subResult = await _repository
                    .IgnoreQueryFilters().Where(predicate).ToListAsync();
                result.AddRange(subResult);
            }
            return result;
        }
        /// <summary>
        /// 获取旧的数据(阶段)
        /// </summary>
        public async Task<List<PlayerCompleteModel>> GetOldStagetimesData(IEnumerable<(int auth, string map, int stage)> list)
        {
            var result = new List<PlayerCompleteModel>();
            int batchSize = 100;
            int total = list.Count();
            for (int i = 0; i < total; i += batchSize)
            {
                var batch = list.Skip(i).Take(batchSize).ToList();
                var predicate = PredicateBuilder.New<PlayerCompleteModel>(false);
                // 处理每一批 batch
                foreach (var item in batch)
                {
                    predicate = predicate.Or(t =>
                        t.Auth == item.auth &&
                        t.MapName == item.map &&
                        t.Type == RecordTypeEnum.Stage &&
                        t.Stage == item.stage
                    );
                }
                var subResult = await _repository
                    .IgnoreQueryFilters().Where(predicate).ToListAsync();
                result.AddRange(subResult);
            }
            return result;
        }
    }
}