using IServices;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Players;
using Model.Models.Main;

namespace ClientWeb.Controllers;

/// <summary>
/// ���
/// </summary>
[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerServices _playerServices;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerServices"></param>
    public PlayerController(IPlayerServices playerServices)
    {
        _playerServices = playerServices;
    }
    /// <summary>
    /// ��ȡ�����Ϣ
    /// </summary>
    [HttpGet("GetPlayerInfo")]
    public async Task<PlayerInfoDto?> GetPlayerInfo(long id)
    {
        return await _playerServices.GetPlayerInfo(id);
    }
    ///// <summary>
    ///// ��ȡ���WRCount
    ///// </summary>
    //[HttpGet("GetPlayerWRCount")]
    //public int GetPlayerWRCount(string id, RecordTypeEnum recordType,string difficulty)
    //{
    //    return _playerServices.GetPlayerWRCount(id, recordType,  difficulty);
    //}
    ///// <summary>
    ///// ��ȡ���WRList
    ///// </summary>
    //[HttpGet("GetPlayerWRList")]
    //public List<PlayerWRDto> GetPlayerWRList(string id, RecordTypeEnum recordType, string difficulty, int pageIndex)
    //{
    //    return _playerServices.GetPlayerWRList(id, recordType,  difficulty, pageIndex);
    //}
    /// <summary>
    /// ��ȡ��������Count
    /// </summary>
    [HttpGet("GetPlayerSucceesCount")]
    public async Task<int> GetPlayerSucceesCount(string id, RecordTypeEnum recordType, string difficulty)
    {
        return await _playerServices.GetPlayerSucceesCount(id, recordType,  difficulty);
    }
    /// <summary>
    /// ��ȡ��������List
    /// </summary>
    [HttpGet("GetPlayerSucceesList")]
    public async Task<List<PlayerSucceesDto>> GetPlayerSucceesList(string id, RecordTypeEnum recordType, string difficulty, int pageIndex)
    {
        return await _playerServices.GetPlayerSucceesList(id, recordType,  difficulty, pageIndex);
    }

    ///// <summary>
    ///// ��ȡ���δ���Count
    ///// </summary>
    //[HttpGet("GetPlayerFailCount")]
    //public async Task<int> GetPlayerFailCount(string id, RecordTypeEnum recordType, string difficulty)
    //{
    //    return await _playerServices.GetPlayerFailCount(id, recordType, difficulty);
    //}
    /// <summary>
    /// ��ȡ���δ���List
    /// </summary>
    [HttpGet("GetPlayerFailList")]
    public async Task<List<PlayerFailDto>> GetPlayerFailList(string id, RecordTypeEnum recordType, string difficulty, int pageIndex)
    {
        return await _playerServices.GetPlayerFailList(id, recordType, difficulty, pageIndex);
    }
}