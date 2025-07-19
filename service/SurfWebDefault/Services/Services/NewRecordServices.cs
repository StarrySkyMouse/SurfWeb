using Microsoft.EntityFrameworkCore;
using Model.Dtos.NewRecords;
using Model.Entitys;
using Repositories.IRepository;
using Services.Base;
using Services.IServices;

namespace Services.Services
{
    public class NewRecordServices : BaseServices<NewRecordModel>, INewRecordServices
    {
        private readonly INewRecordRepository _repository;
        private readonly IMapRepository _mapRepository;
        private readonly IPlayerCompleteRepository _playerCompleteRepository;
        public NewRecordServices(INewRecordRepository repository,
            IMapRepository mapRepository,
            IPlayerCompleteRepository playerCompleteRepository
            ) : base(repository)
        {
            _repository = repository;
            _mapRepository = mapRepository;
            _playerCompleteRepository = playerCompleteRepository;
        }
        /// <summary>
        /// 获取最新纪录
        /// </summary>
        public async Task<List<NewRecordDto>> GetNewRecordList(RecordTypeEnum recordType)
        {
            return await _repository.Select(t => new NewRecordDto
            {
                PlayerId = t.PlayerId,
                PlayerName = t.PlayerName,
                MapId = t.MapId,
                MapName = t.MapName,
                Type = t.Type,
                Notes = t.Notes,
                Time = t.Time,
                Date = t.Date
            }).Where(t => t.Type == recordType)
            .OrderByDescending(t => t.Date)
            .Take(10)
            .ToListAsync();
        }
        /// <summary>
        /// 获取新增地图
        /// </summary>
        public async Task<List<NewMapDto>> GetNewMapList()
        {
            return await _mapRepository.Select(t => new NewMapDto
            {
                Id = t.Id,
                Name = t.Name,
                Difficulty = t.Difficulty,
                Img = t.Img,
                CreateTime = t.CreateTime,
            })
            .OrderByDescending(t => t.CreateTime)
            .Take(10)
            .ToListAsync();
        }
        /// <summary>
        /// 热门地图
        /// </summary>
        public async Task<List<PopularMapDto>> GetPopularMapList()
        {
            return await _mapRepository.Select(t => new PopularMapDto
            {
                Id = t.Id,
                Name = t.Name,
                Difficulty = t.Difficulty,
                SurcessNumber = t.SurcessNumber,
            })
            .OrderByDescending(t => t.SurcessNumber)
            .Take(10)
            .ToListAsync();
        }
        /// <summary>
        /// 更新最新记录数据
        /// </summary>>
        public async Task UpdateNewRecord()
        {
            var mainNewRecord = await _playerCompleteRepository
                .Where(t => t.PlayerId != null && t.MapId != null && t.Type == RecordTypeEnum.Main)
                .OrderByDescending(t => t.Date)
                .Take(10)
                .ToListAsync();
            var bountyNewRecord = await _playerCompleteRepository
                .Where(t => t.PlayerId != null && t.MapId != null && t.Type == RecordTypeEnum.Bounty)
                .OrderByDescending(t => t.Date)
                .Take(10)
                .ToListAsync();
            var stageNewRecord = await _playerCompleteRepository
                .Where(t => t.PlayerId != null && t.MapId != null && t.Type == RecordTypeEnum.Stage)
                .OrderByDescending(t => t.Date)
                .Take(10)
                .ToListAsync();
            using var transaction = _repository.BeginTransaction();
            try
            {
                //删除数据
                _repository.DeleteAll();
                _repository.SaveChanges();
                //插入主线记录
                if (mainNewRecord.Any())
                {
                    var mapInfo = await _mapRepository
                        .Where(t => mainNewRecord.Select(a => a.MapId).Contains(t.Id))
                        .Select(t => new
                        {
                            t.Id,
                            t.Difficulty
                        })
                        .ToListAsync();
                    _repository.Inserts(mainNewRecord.Select(t => new NewRecordModel()
                    {
                        Id = null,
                        PlayerId = t.PlayerId,
                        PlayerName = t.PlayerName,
                        MapId = t.MapId,
                        MapName = t.MapName,
                        Type = RecordTypeEnum.Main,
                        Notes = mapInfo.First(a => t.MapId == a.Id).Difficulty,
                        Time = t.Time,
                        Date = t.Date
                    }));
                    _repository.SaveChanges();
                }
                //插入奖励记录
                if (bountyNewRecord.Any())
                {
                    _repository.Inserts(bountyNewRecord.Select(t => new NewRecordModel()
                    {
                        Id = null,
                        PlayerId = t.PlayerId,
                        PlayerName = t.PlayerName,
                        MapId = t.MapId,
                        MapName = t.MapName,
                        Type = RecordTypeEnum.Bounty,
                        Notes = (t.Stage ?? -1).ToString(),
                        Time = t.Time,
                        Date = t.Date
                    }));
                    _repository.SaveChanges();
                }
                //插入阶段记录
                if (stageNewRecord.Any())
                {
                    _repository.Inserts(stageNewRecord.Select(t => new NewRecordModel()
                    {
                        Id = null,
                        PlayerId = t.PlayerId,
                        PlayerName = t.PlayerName,
                        MapId = t.MapId,
                        MapName = t.MapName,
                        Type = RecordTypeEnum.Stage,
                        Notes = (t.Stage ?? -1).ToString(),
                        Time = t.Time,
                        Date = t.Date
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