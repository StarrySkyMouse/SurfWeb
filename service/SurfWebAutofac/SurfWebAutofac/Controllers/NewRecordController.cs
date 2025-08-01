using IServices.Main;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.NewRecords;
using Model.Models.Main;

namespace SurfWebAutofac.Controllers;

/// <summary>
///     �¼�¼������
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
    ///     ��ȡ���¼�¼
    /// </summary>
    [HttpGet("GetNewRecordList")]
    public async Task<List<NewRecordDto>> GetNewRecordList(RecordTypeEnum recordType)
    {
        return await _mapServices.GetNewRecordList(recordType);
    }

    /// <summary>
    ///     ��ȡ������ͼ
    /// </summary>
    [HttpGet("GetNewMapList")]
    public async Task<List<NewMapDto>> GetNewMapList()
    {
        return await _mapServices.GetNewMapList();
    }

    /// <summary>
    ///     ��ȡ���ŵ�ͼ
    /// </summary>
    [HttpGet("GetPopularMapList")]
    public async Task<List<PopularMapDto>> GetPopularMapList()
    {
        return await _mapServices.GetPopularMapList();
    }
}