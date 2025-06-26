using Core.Dto;
using Core.IRepository;
using Core.IRepository.Base;
using Core.IServices;
using Core.Models;
using Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Dto.Maps;
using Core.Utils.Extensions;

namespace Core.Services
{
    public class MapServices : BaseServices<MapModel>, IMapServices
    {
        private readonly IMapRepository _repository;
        private readonly IPlayerCompleteRepository _playerCompleteRepository;
        public MapServices(IMapRepository repository,
            IPlayerCompleteRepository playerCompleteRepository) : base(repository)
        {
            _repository = repository;
            _playerCompleteRepository = playerCompleteRepository;
        }
        /// <summary>
        /// 获取地图列表
        /// </summary>
        public async Task<int> GetMapCount(string? difficulty, string? search)
        {
            return await GetMapQueryable(difficulty, search).CountAsync();
        }
        /// <summary>
        /// 获取地图信息
        /// </summary>
        public async Task<MapDto?> GetMapInfo(string id)
        {
            return await _repository
                .Select(
                    t => new MapDto()
                    {
                        Id = t.Id,
                        WRPlayerId = t.WRPlayerId,
                        WRPlayerName = t.WRPlayerName,
                        WRTime = t.WRTime,
                        WRDate = t.WRDate,
                        Name = t.Name,
                        Difficulty = t.Difficulty,
                        Img = t.Img,
                        SurcessNumber = t.SurcessNumber,
                        BonusNumber = t.BonusNumber,
                        StageNumber = t.StageNumber
                    }).FirstOrDefaultAsync(t => t.Id == id);
        }
        /// <summary>
        /// 获取地图列表
        /// </summary>
        public async Task<List<MapListDto>> GetMapList(string? difficulty, string? search, int pageIndex)
        {
            return await GetMapQueryable(difficulty, search)
                .PageData(pageIndex, 10)
                .Select(t => new MapListDto()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Difficulty = t.Difficulty,
                    Img = t.Img,
                }).ToListAsync();
        }
        private IQueryable<MapModel> GetMapQueryable(string? difficulty, string? search)
        {
            return _repository
                .WhereIf(!string.IsNullOrWhiteSpace(difficulty), t => t.Difficulty.ToUpper() == difficulty.ToUpper().Trim())
                .WhereIf(!string.IsNullOrWhiteSpace(search), t => t.Name.ToUpper().Contains(search.ToUpper()));
        }

        /// <summary>
        /// 获取地图前100数量
        /// </summary>
        public async Task<int> GetMapTop100Count(string id, RecordTypeEnum recordType)
        {
            var result = await GetMapTop100Queryable(id, recordType).CountAsync();
            return result > 100 ? 100 : result;
        }
        /// <summary>
        /// 获取地图前100
        /// </summary>
        public async Task<List<MapTop100Dto>> GetMapTop100List(string id, RecordTypeEnum recordType, int pageIndex)
        {
            return await GetMapTop100Queryable(id, recordType)
                .PageData(pageIndex, 10)
                .Select(t => new MapTop100Dto()
                {
                    PlayerId = t.PlayerId,
                    PlayerName = t.PlayerName,
                    Stage = t.Stage,
                    Time = t.Time,
                    Date = t.Date,
                }).ToListAsync();
        }
        private IQueryable<PlayerCompleteModel> GetMapTop100Queryable(string id, RecordTypeEnum recordType)
        {
            return _playerCompleteRepository.Where(t => t.MapId == id && t.Type == recordType);
        }
        /// <summary>
        /// 获取地图缓存列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<MapCacheDto>> GetMapCacheList()
        {
            return await _repository.Select(t => new MapCacheDto()
            {
                Id = t.Id,
                Name = t.Name,
                BonusNumber = t.BonusNumber,
                StageNumber = t.StageNumber,
            }).ToListAsync();
        }
    }
}
