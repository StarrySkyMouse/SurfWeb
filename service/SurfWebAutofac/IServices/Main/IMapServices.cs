using IServices.Main.Base;
using Model.Caches;
using Model.Dtos.Maps;
using Model.Dtos.NewRecords;
using Model.Models.Main;

namespace IServices.Main;

public interface IMapServices : IMainBaseServices<MapModel>
{
    /// <summary>
    ///     获取地图信息
    /// </summary>
    Task<MapDto?> GetMapInfoById(long id);
    /// <summary>
    /// 通过名字获取地图信息列表
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    Task<MapModel?> GetMapInfoByName(string names);
    /// <summary>
    /// 通过名字列表获取地图信息列表
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    Task<List<MapModel>> GetMapInfoByNameList(IEnumerable<string> names);

    /// <summary>
    ///     获取地图列表
    /// </summary>
    Task<int> GetMapCount(string? difficulty, string? search);

    /// <summary>
    ///     获取地图列表
    /// </summary>
    Task<List<MapListDto>> GetMapList(string? difficulty, string? search, int pageIndex);

    /// <summary>
    ///     获取地图前100数量
    /// </summary>
    Task<int> GetMapTop100Count(long id, RecordTypeEnum recordType, int? stage);

    /// <summary>
    ///     获取地图前100
    /// </summary>
    Task<List<MapTop100Dto>> GetMapTop100List(long id, RecordTypeEnum recordType, int? stage, int pageIndex);

    /// <summary>
    ///通过地图名称获取地图ID列表
    /// </summary>
    Task<Dictionary<string, long>> GetMapIdListByName(List<string> mapNameList);

    /// <summary>
    ///统计地图完成人数
    /// </summary>
    Task UpdateSucceesNumber();

    /// <summary>
    ///     获取地图wr列表
    /// </summary>
    Task<List<MapWrCache>> GetMapWrList(RecordTypeEnum recordType);

    /// <summary>
    /// 获取地图信息Main
    /// </summary>
    Task<List<MapMainCache>> GetMapMainList();

    /// <summary>
    /// 获取地图信息Bounty
    /// </summary>
    Task<List<MapBountyOrStageCache>> GetMapBountyList();

    /// <summary>
    /// 获取地图信息Bounty
    /// </summary>
    Task<List<MapBountyOrStageCache>> GetMapStageList();
    /// <summary>
    ///     获取最新纪录
    /// </summary>
    Task<List<NewRecordDto>> GetNewRecordList(RecordTypeEnum recordType);

    /// <summary>
    ///     获取新增地图
    /// </summary>
    Task<List<NewMapDto>> GetNewMapList();

    /// <summary>
    ///     热门地图
    /// </summary>
    Task<List<PopularMapDto>> GetPopularMapList();
}