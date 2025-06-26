using Core.Dto;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Client.Controllers
{
    /// <summary>
    /// 排行
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RankingController : ControllerBase
    {
        private readonly IRankingServices _rankingServices;
        /// <summary>
        /// 
        /// </summary>
        public RankingController(IRankingServices rankingServices)
        {
            _rankingServices = rankingServices;
        }
        /// <summary>
        /// 获取排行信息排行
        /// </summary>
        [HttpGet("GetRankingList")]
        public async Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType)
        {
            return await _rankingServices.GetRankingList(rankingType);
        }
    }
}
