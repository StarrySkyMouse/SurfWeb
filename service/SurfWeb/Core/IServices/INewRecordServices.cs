using Core.Dto;
using Core.Dto.NewRecords;
using Core.IServices.Base;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface INewRecordServices : IBaseServices<NewRecordModel>
    {
        /// <summary>
        /// 获取新增地图
        /// </summary>
        Task<List<NewMapDto>> GetNewMapList();
        /// <summary>
        /// 获取最新纪录
        /// </summary>
        Task<List<NewRecordDto>> GetNewRecordList(RecordTypeEnum recordType);
        /// <summary>
        /// 获取热门地图
        /// </summary>
        Task<List<PopularMapDto>> GetPopularMapList();
    }
}
