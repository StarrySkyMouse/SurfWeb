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
    private readonly INewRecordServices _newRecordServices;

    /// <summary>
    /// </summary>
    public NewRecordController(INewRecordServices newRecordServices)
    {
        _newRecordServices = newRecordServices;
    }

    /// <summary>
    ///     ��ȡ���¼�¼
    /// </summary>
    [HttpGet("GetNewRecordList")]
    public async Task<List<NewRecordDto>> GetNewRecordList(RecordTypeEnum recordType)
    {
        return await _newRecordServices.GetNewRecordList(recordType);
    }

    /// <summary>
    ///     ��ȡ������ͼ
    /// </summary>
    [HttpGet("GetNewMapList")]
    public async Task<List<NewMapDto>> GetNewMapList()
    {
        return await _newRecordServices.GetNewMapList();
    }

    /// <summary>
    ///     ��ȡ���ŵ�ͼ
    /// </summary>
    [HttpGet("GetPopularMapList")]
    public async Task<List<PopularMapDto>> GetPopularMapList()
    {
        return await _newRecordServices.GetPopularMapList();
    }
}