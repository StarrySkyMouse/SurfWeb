using Microsoft.EntityFrameworkCore;
using Model.Cahces;
using Model.Dtos.Rankings;
using Model.Entitys;
using Repositories.IRepository;
using Services.Base;
using Services.IServices;

namespace Services.Services
{
    public class RankingServices : BaseServices<RankingModel>, IRankingServices
    {
        private readonly IRankingRepository _repository;
        private readonly IPlayerCompleteRepository _playerCompleteRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly DataCache _dataCache;
        public RankingServices(IRankingRepository repository,
            IPlayerCompleteRepository playerCompleteRepository,
            IPlayerRepository playerRepository,
            DataCache dataCache) : base(repository)
        {
            _repository = repository;
            _playerCompleteRepository = playerCompleteRepository;
            _playerRepository = playerRepository;
            _dataCache = dataCache;
        }
        public async Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType)
        {
            var ranKingList = await _repository.Select(t => new RankingDto()
            {
                Type = t.Type,
                Rank = t.Rank,
                PlayerId = t.PlayerId,
                PlayerName = t.PlayerName,
                Value = t.Value,
            }).Where(t => t.Rank <= 10 && t.Type == rankingType)
            .OrderBy(t => t.Rank).ToListAsync();
            //只有在积分模式下需要查询通关情况
            if (rankingType == RankingTypeEnum.Integral)
            {
                //查询玩家主线通关情况
                var playerCompleteList = await _playerCompleteRepository
                    .Where(t =>
                        ranKingList.Select(a => a.PlayerId).Contains(t.PlayerId) &&
                        t.Type == RecordTypeEnum.Main)
                    .GroupBy(t => t.PlayerId)
                    .Select(t => new
                    {
                        PlayerId = t.Key,
                        SucceedNumber = t.Count()
                    })
                    .ToListAsync();
                var sumMap = _dataCache.MapSnapshot.MapMainList.Count();
                foreach (var item in ranKingList)
                {
                    var playerComplete = playerCompleteList.FirstOrDefault(t => t.PlayerId == item.PlayerId);
                    item.Progress = (playerComplete != null ? playerComplete.SucceedNumber : 0) + $"/{sumMap}";
                }
            }
            return ranKingList;
        }
        /// <summary>
        /// 更新排行榜数据
        /// </summary>
        public async Task UpdateRanking()
        {
            //积分
            var integralRaking = await _playerRepository
                .OrderByDescending(t => t.Integral)
               .Take(10)
               .ToListAsync();
            //主线
            var wRNumberRaking = await _playerRepository
                .OrderByDescending(t => t.WRNumber)
                .Take(10)
                .ToListAsync();
            //奖励
            var bWRNumberRaking = await _playerRepository
               .OrderByDescending(t => t.BWRNumber)
               .Take(10)
               .ToListAsync();
            //阶段
            var sWRNumber = await _playerRepository
                .OrderByDescending(t => t.SWRNumber)
                .Take(10)
                .ToListAsync();
            using var transaction = _repository.BeginTransaction();
            try
            {
                //删除数据
                _repository.DeleteAll();
                _repository.SaveChanges();
                if (integralRaking.Any())
                {
                    _repository.Inserts(integralRaking.Select(t => new RankingModel()
                    {
                        Id = null,
                        Type = RankingTypeEnum.Integral,
                        Rank = integralRaking.IndexOf(t) + 1,
                        PlayerId = t.Id,
                        PlayerName = t.Name,
                        Value = t.Integral
                    }));
                    _repository.SaveChanges();
                }
                if (wRNumberRaking.Any())
                {
                    _repository.Inserts(wRNumberRaking.Select(t => new RankingModel()
                    {
                        Id = null,
                        Type = RankingTypeEnum.MainWR,
                        Rank = wRNumberRaking.IndexOf(t) + 1,
                        PlayerId = t.Id,
                        PlayerName = t.Name,
                        Value = t.WRNumber
                    }));
                    _repository.SaveChanges();
                }
                if (bWRNumberRaking.Any())
                {
                    _repository.Inserts(bWRNumberRaking.Select(t => new RankingModel()
                    {
                        Id = null,
                        Type = RankingTypeEnum.BountyWR,
                        Rank = bWRNumberRaking.IndexOf(t) + 1,
                        PlayerId = t.Id,
                        PlayerName = t.Name,
                        Value = t.BWRNumber
                    }));
                    _repository.SaveChanges();
                }
                if (sWRNumber.Any())
                {
                    _repository.Inserts(sWRNumber.Select(t => new RankingModel()
                    {
                        Id = null,
                        Type = RankingTypeEnum.StageWR,
                        Rank = sWRNumber.IndexOf(t) + 1,
                        PlayerId = t.Id,
                        PlayerName = t.Name,
                        Value = t.SWRNumber
                    }));
                    _repository.SaveChanges();
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}