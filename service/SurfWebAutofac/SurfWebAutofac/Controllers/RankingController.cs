using IServices.Main;
using Microsoft.AspNetCore.Mvc;
using Model.Dtos.Rankings;
using Model.Models.Main;

namespace SurfWebAutofac.Controllers;

/// <summary>
///     ����
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
    ///     ��ȡ������Ϣ����
    /// </summary>
    [HttpGet("GetRankingList")]
    public async Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType)
    {
        return await _playerCompleteServices.GetRankingList(rankingType);
    }
}