using Model.Dtos.NewRecords;
using Model.Entitys;
using Services.Base;

namespace Services.IServices
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
        //更新新的
        Task UpdateNewRecord();
    }
}
