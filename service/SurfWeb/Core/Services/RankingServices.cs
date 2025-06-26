using Core.Dto;
using Core.IRepository;
using Core.IRepository.Base;
using Core.IServices;
using Core.Models;
using Core.Services.Base;
using Core.Utils.GlobalParams;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class RankingServices : BaseServices<RankingModel>, IRankingServices
    {
        private readonly IRankingRepository _repository;
        private readonly IPlayerCompleteRepository _playerCompleteModels;
        private readonly DataCache _dataCache;
        public RankingServices(IRankingRepository repository,
            IPlayerCompleteRepository playerCompleteModels,
            DataCache dataCache) : base(repository)
        {
            _repository = repository;
            _playerCompleteModels = playerCompleteModels;
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
                Integral = t.Value,
            }).Where(t => t.Rank <= 10 && t.Type == rankingType)
            .OrderBy(t => t.Rank).ToListAsync();
            //只有在积分模式下需要查询通关情况
            if (rankingType == RankingTypeEnum.Integral)
            {
                //查询玩家主线通关情况
                var playerCompleteList = await _playerCompleteModels
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
                var sumMap = _dataCache.Snapshot.MapMainList.Count();
                foreach (var item in ranKingList)
                {
                    var playerComplete = playerCompleteList.FirstOrDefault(t => t.PlayerId == item.PlayerId);
                    item.Progress = (playerComplete != null ? playerComplete.SucceedNumber : 0) + $"/{sumMap}";
                }
            }
            return ranKingList;
        }
    }
}