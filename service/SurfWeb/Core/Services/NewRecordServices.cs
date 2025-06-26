using Core.Dto;
using Core.Dto.NewRecords;
using Core.IRepository;
using Core.IRepository.Base;
using Core.IServices;
using Core.Models;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class NewRecordServices : BaseServices<NewRecordModel>, INewRecordServices
    {
        private readonly INewRecordRepository _repository;
        private readonly IMapRepository _mapRepository;
        public NewRecordServices(INewRecordRepository repository, IMapRepository mapRepository) : base(repository)
        {
            _repository = repository;
            _mapRepository = mapRepository;
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
                Notes=t.Notes,
                Time = t.Time,
                Date = t.CreateTime
            }).Where(t => t.Type == recordType)
            .OrderBy(t => t.Date)
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
                Img = t.Img,
                SurcessNumber = t.SurcessNumber,
            })
            .OrderBy(t => t.SurcessNumber)
            .Take(10)
            .ToListAsync();
        }
    }
}