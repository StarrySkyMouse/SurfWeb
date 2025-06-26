using Core.Dto;
using Core.IServices;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Client.Controllers
{
    /// <summary>
    /// ����
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
        /// ��ȡ������Ϣ����
        /// </summary>
        [HttpGet("GetRankingList")]
        public async Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType)
        {
            return await _rankingServices.GetRankingList(rankingType);
        }
    }
}
