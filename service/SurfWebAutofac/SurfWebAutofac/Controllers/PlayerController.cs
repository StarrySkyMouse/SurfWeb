using IServices.Main;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Players;
using Model.Models.Main;

namespace SurfWebAutofac.Controllers;

/// <summary>
///     玩家
/// </summary>
[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerServices _playerServices;

    /// <summary>
    /// </summary>
    /// <param name="playerServices"></param>
    public PlayerController(IPlayerServices playerServices)
    {
        _playerServices = playerServices;
    }

    /// <summary>
    ///     获取玩家信息
    /// </summary>
    [HttpGet("GetPlayerInfo")]
    public async Task<PlayerInfoDto?> GetPlayerInfo(long id)
    {
        return await _playerServices.GetPlayerInfo(id);
    }
    /// <summary>
    ///     获取玩家已完成Count
    /// </summary>
    [HttpGet("GetPlayerSucceesCount")]
    public async Task<int> GetPlayerSucceesCount(long id, RecordTypeEnum recordType, string difficulty)
    {
        return await _playerServices.GetPlayerSucceesCount(id, recordType, difficulty);
    }

    /// <summary>
    ///     获取玩家已完成List
    /// </summary>
    [HttpGet("GetPlayerSucceesList")]
    public async Task<List<PlayerSucceesDto>> GetPlayerSucceesList(long id, RecordTypeEnum recordType,
        string difficulty, int pageIndex)
    {
        return await _playerServices.GetPlayerSucceesList(id, recordType, difficulty, pageIndex);
    }

    /// <summary>
    /// 获取玩家未完成Count
    /// </summary>
    [HttpGet("GetPlayerFailCount")]
    public async Task<int> GetPlayerFailCount(long id, RecordTypeEnum recordType, string difficulty)
    {
        return await _playerServices.GetPlayerFailCount(id, recordType, difficulty);
    }
    /// <summary>
    ///     获取玩家未完成List
    /// </summary>
    [HttpGet("GetPlayerFailList")]
    public async Task<List<PlayerFailDto>> GetPlayerFailList(long id, RecordTypeEnum recordType, string difficulty,
        int pageIndex)
    {
        return await _playerServices.GetPlayerFailList(id, recordType, difficulty, pageIndex);
    }
}