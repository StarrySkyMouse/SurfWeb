using IServices;
using Model.Dtos.Rankings;
using Model.Models;
using Repository.BASE;
using Services.Base;

namespace Services
{
    public class RankingServices : BaseServices<RankingModel>, IRankingServices
    {
        private readonly IBaseRepository<RankingModel> _rankingRepository;
        private readonly IBaseRepository<PlayerCompleteModel> _playerCompleteRepository;   
        private readonly IBaseRepository<PlayerModel> _playerRepository;
        public RankingServices(IBaseRepository<RankingModel> rankingRepository,
            IBaseRepository<PlayerCompleteModel> playerCompleteRepository,
            IBaseRepository<PlayerModel> playerRepository)
        {
            _rankingRepository = rankingRepository;
            _playerCompleteRepository = playerCompleteRepository;
            _playerRepository = playerRepository;
        }
        public async Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType)
        {
            var ranKingList = await _rankingRepository.Queryable().Select(t => new RankingDto()
            {
                Type = t.Type,
                Rank = t.Rank,
                PlayerId = t.PlayerId,
                PlayerName = t.PlayerName,
                Value = t.Value,
            }).Where(t => t.Rank <= 10 && t.Type == rankingType)
            .OrderBy(t => t.Rank).ToListAsync();
            //只有在积分模式下需要查询通关情况
            //if (rankingType == RankingTypeEnum.Integral)
            //{
            //    //查询玩家主线通关情况
            //    var playerCompleteList = await _playerCompleteRepository.Queryable()
            //        .Where(t =>
            //            ranKingList.Select(a => a.PlayerId).Contains(t.PlayerId) &&
            //            t.Type == RecordTypeEnum.Main)
            //        .GroupBy(t => t.PlayerId)
            //        .Select(t => new
            //        {
            //            PlayerId = t.Key,
            //            SucceedNumber = t.Count()
            //        })
            //        .ToListAsync();
            //    var sumMap = _dataCache.MapSnapshot.MapMainList.Count();
            //    foreach (var item in ranKingList)
            //    {
            //        var playerComplete = playerCompleteList.FirstOrDefault(t => t.PlayerId == item.PlayerId);
            //        item.Progress = (playerComplete != null ? playerComplete.SucceedNumber : 0) + $"/{sumMap}";
            //    }
            //}
            return ranKingList;
        }
        /// <summary>
        /// 更新排行榜数据
        /// </summary>
        public async Task UpdateRanking()
        {
            //积分
            var integralRaking = await _playerRepository.Queryable()
                .OrderByDescending(t => t.Integral)
               .Take(10)
               .ToListAsync();
            //主线
            var wRNumberRaking = await _playerRepository.Queryable()
                .OrderByDescending(t => t.WRNumber)
                .Take(10)
                .ToListAsync();
            //奖励
            var bWRNumberRaking = await _playerRepository.Queryable()
               .OrderByDescending(t => t.BWRNumber)
               .Take(10)
               .ToListAsync();
            //阶段
            var sWRNumber = await _playerRepository.Queryable()
                .OrderByDescending(t => t.SWRNumber)
                .Take(10)
                .ToListAsync();
            //using var transaction = _rankingRepository.BeginTransaction();
            //try
            //{
            //    //删除数据
            //    _rankingRepository.DeleteAll();
            //    _rankingRepository.SaveChanges();
            //    if (integralRaking.Any())
            //    {
            //        _rankingRepository.Inserts(integralRaking.Select(t => new RankingModel()
            //        {
            //            Id = null,
            //            Type = RankingTypeEnum.Integral,
            //            Rank = integralRaking.IndexOf(t) + 1,
            //            PlayerId = t.Id,
            //            PlayerName = t.Name,
            //            Value = t.Integral
            //        }));
            //        _rankingRepository.SaveChanges();
            //    }
            //    if (wRNumberRaking.Any())
            //    {
            //        _rankingRepository.Inserts(wRNumberRaking.Select(t => new RankingModel()
            //        {
            //            Id = null,
            //            Type = RankingTypeEnum.MainWR,
            //            Rank = wRNumberRaking.IndexOf(t) + 1,
            //            PlayerId = t.Id,
            //            PlayerName = t.Name,
            //            Value = t.WRNumber
            //        }));
            //        _rankingRepository.SaveChanges();
            //    }
            //    if (bWRNumberRaking.Any())
            //    {
            //        _rankingRepository.Inserts(bWRNumberRaking.Select(t => new RankingModel()
            //        {
            //            Id = null,
            //            Type = RankingTypeEnum.BountyWR,
            //            Rank = bWRNumberRaking.IndexOf(t) + 1,
            //            PlayerId = t.Id,
            //            PlayerName = t.Name,
            //            Value = t.BWRNumber
            //        }));
            //        _rankingRepository.SaveChanges();
            //    }
            //    if (sWRNumber.Any())
            //    {
            //        _rankingRepository.Inserts(sWRNumber.Select(t => new RankingModel()
            //        {
            //            Id = null,
            //            Type = RankingTypeEnum.StageWR,
            //            Rank = sWRNumber.IndexOf(t) + 1,
            //            PlayerId = t.Id,
            //            PlayerName = t.Name,
            //            Value = t.SWRNumber
            //        }));
            //        _rankingRepository.SaveChanges();
            //    }
            //    await transaction.CommitAsync();
            //}
            //catch
            //{
            //    await transaction.RollbackAsync();
            //    throw;
            //}
        }
    }
}