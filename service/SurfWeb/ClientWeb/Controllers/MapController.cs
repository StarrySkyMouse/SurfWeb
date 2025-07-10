using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Maps;
using Model.Entitys;
using Services.IServices;

namespace ClientWeb.Controllers;

/// <summary>
/// 地图控制器
/// </summary>
[ApiController]
[Route("[controller]")]
public class MapController : ControllerBase
{
    private readonly IMapServices _mapServices;
    /// <summary>
    /// 
    /// </summary>
    public MapController(IMapServices mapServices)
    {
        _mapServices = mapServices;
    }
    /// <summary>
    /// 获取地图信息
    /// </summary>
    [HttpGet("GetMapInfo")]
    public async Task<MapDto?> GetMapInfo(string id)
    {
        return await _mapServices.GetMapInfoById(id);
    }
    /// <summary>
    /// 获取地图列表Count
    /// </summary>
    [HttpGet("GetMapCount")]
    public async Task<int> GetMapCount(string? difficulty, string? search)
    {
        return await _mapServices.GetMapCount(difficulty, search);
    }
    /// <summary>
    /// 获取地图列表List
    /// </summary>
    [HttpGet("GetMapList")]
    public async Task<List<MapListDto>> GetMapList(string? difficulty, string? search, int pageIndex = 1)
    {
        return await _mapServices.GetMapList(difficulty, search, pageIndex);
    }
    /// <summary>
    /// 获取地图前100Count
    /// </summary>
    [HttpGet("GetMapTop100Count")]
    public async Task<int> GetMapTop100Count(string id, RecordTypeEnum recordType)
    {
        return await _mapServices.GetMapTop100Count(id, recordType);
    }
    /// <summary>
    /// 获取地图前100List
    /// </summary>
    [HttpGet("GetMapTop100List")]
    public async Task<List<MapTop100Dto>> GetMapTop100List(string id, RecordTypeEnum recordType, int pageIndex)
    {
        return await _mapServices.GetMapTop100List(id, recordType, pageIndex);
    }
}