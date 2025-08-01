using IServices.Main;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.NewRecords;
using Model.Models.Main;

namespace SurfWebAutofac.Controllers;

/// <summary>
///     新纪录控制器
/// </summary>
[ApiController]
[Route("[controller]")]
public class NewRecordController : ControllerBase
{
    private readonly IMapServices _mapServices;

    /// <summary>
    /// </summary>
    public NewRecordController(IMapServices mapServices)
    {
        _mapServices = mapServices;
    }

    /// <summary>
    ///     获取最新纪录
    /// </summary>
    [HttpGet("GetNewRecordList")]
    public async Task<List<NewRecordDto>> GetNewRecordList(RecordTypeEnum recordType)
    {
        return await _mapServices.GetNewRecordList(recordType);
    }

    /// <summary>
    ///     获取新增地图
    /// </summary>
    [HttpGet("GetNewMapList")]
    public async Task<List<NewMapDto>> GetNewMapList()
    {
        return await _mapServices.GetNewMapList();
    }

    /// <summary>
    ///     获取热门地图
    /// </summary>
    [HttpGet("GetPopularMapList")]
    public async Task<List<PopularMapDto>> GetPopularMapList()
    {
        return await _mapServices.GetPopularMapList();
    }
}