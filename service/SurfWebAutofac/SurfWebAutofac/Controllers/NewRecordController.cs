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
    private readonly INewRecordServices _newRecordServices;

    /// <summary>
    /// </summary>
    public NewRecordController(INewRecordServices newRecordServices)
    {
        _newRecordServices = newRecordServices;
    }

    /// <summary>
    ///     获取最新纪录
    /// </summary>
    [HttpGet("GetNewRecordList")]
    public async Task<List<NewRecordDto>> GetNewRecordList(RecordTypeEnum recordType)
    {
        return await _newRecordServices.GetNewRecordList(recordType);
    }

    /// <summary>
    ///     获取新增地图
    /// </summary>
    [HttpGet("GetNewMapList")]
    public async Task<List<NewMapDto>> GetNewMapList()
    {
        return await _newRecordServices.GetNewMapList();
    }

    /// <summary>
    ///     获取热门地图
    /// </summary>
    [HttpGet("GetPopularMapList")]
    public async Task<List<PopularMapDto>> GetPopularMapList()
    {
        return await _newRecordServices.GetPopularMapList();
    }
}