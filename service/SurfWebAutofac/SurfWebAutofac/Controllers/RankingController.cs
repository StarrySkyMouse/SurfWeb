using IServices.Main;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Rankings;
using Model.Models.Main;

namespace SurfWebAutofac.Controllers;

/// <summary>
///     排行
/// </summary>
[ApiController]
[Route("[controller]")]
public class RankingController : ControllerBase
{
    private readonly IPlayerCompleteServices _playerCompleteServices;

    /// <summary>
    /// </summary>
    public RankingController(IPlayerCompleteServices playerCompleteServices)
    {
        _playerCompleteServices = playerCompleteServices;
    }

    /// <summary>
    ///     获取排行信息排行
    /// </summary>
    [HttpGet("GetRankingList")]
    public async Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType)
    {
        return await _playerCompleteServices.GetRankingList(rankingType);
    }
}