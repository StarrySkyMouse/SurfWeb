using Core.Dto.Players;
using Core.Utils.Extensions;
using Core.IRepository;
using Core.IRepository.Base;
using Core.IServices;
using Core.Models;
using Core.Repositories;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utils.GlobalParams;

namespace Core.Services
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
        public async Task<int> GetPlayerWRCount(string id, RecordTypeEnum recordType)
        {
            return await GetPlayerQueryable(id, recordType).CountAsync();
        }
        /// <summary>
        /// 获取玩家WRList
        /// </summary>
        public async Task<List<PlayerWRDto>> GetPlayerWRList(string id, RecordTypeEnum recordType, int pageIndex)
        {
            return await GetPlayerQueryable(id, recordType)
                .PageData(pageIndex, 10)
                .OrderByDescending(t => t.Date)
                .Select(t => new PlayerWRDto()
                {
                    MapId = t.MapId,
                    MapName = t.MapName,
                    Time = t.Time,
                    Date = t.Date
                })
                .ToListAsync();
        }
        private IQueryable<PlayerCompleteModel> GetPlayerQueryable(string id, RecordTypeEnum recordType)
        {
            return _playerCompleteRepository.Where(t => t.PlayerId == id && t.Type == recordType && t.IsWR);
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
            return await GetPlayerSucceesQueryable(id, recordType)
                .PageData(pageIndex, 10)
                .OrderByDescending(t => t.Date)
                .Select(t => new PlayerSucceesDto()
                {
                    MapId = t.MapId,
                    MapName = t.MapName,
                    Stage = t.Stage,
                    Time = t.Time,
                    Date = t.Date
                })
                .ToListAsync();
        }
        private IQueryable<PlayerCompleteModel> GetPlayerSucceesQueryable(string id, RecordTypeEnum recordType)
        {
            return _playerCompleteRepository.Where(t => t.PlayerId == id && t.Type == recordType);
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
                return _dataCache.Snapshot.MapMainList
                    .Where(t => !list.Contains(t.Id))
                    .OrderBy(t => t.Name)
                    .Skip((pageIndex - 1) * 10)
                    .Take(10)
                    .Select(t => new PlayerFailDto()
                    {
                        MapId = t.Id,
                        MapName = t.Name
                    }).ToList();
            }
            else if (recordType == RecordTypeEnum.Bounty)
            {
                var list = await _playerCompleteRepository
                            .Where(a => a.PlayerId == id && a.Type == recordType)
                            .Select(a => a.MapId + "@@" + a.Stage).ToListAsync();
                return _dataCache.Snapshot.MapBountyList
                    //获取未完成的阶段
                    .Where(t => !list.Contains(t.Id + "@@" + t.Stage))
                    .OrderBy(t => t.Name)
                    .Skip((pageIndex - 1) * 10)
                    .Take(10)
                    .Select(t => new PlayerFailDto()
                    {
                        MapId = t.Id,
                        MapName = t.Name,
                        Stage = t.Stage
                    }).ToList();
            }
            else if (recordType == RecordTypeEnum.Stage)
            {
                var list = await _playerCompleteRepository
                            .Where(a => a.PlayerId == id && a.Type == recordType)
                            .Select(a => a.MapId + "@@" + a.Stage).ToListAsync();
                return _dataCache.Snapshot.MapStageList
                    //获取未完成的阶段
                    .Where(t => !list.Contains(t.Id + "@@" + t.Stage))
                    .OrderBy(t => t.Name)
                    .Skip((pageIndex - 1) * 10)
                    .Take(10)
                    .Select(t => new PlayerFailDto()
                    {
                        MapId = t.Id,
                        MapName = t.Name,
                        Stage = t.Stage
                    }).ToList();
            }
            return await Task.FromResult(new List<PlayerFailDto>());
        }
    }
}
