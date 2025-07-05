using Core.Dto;
using Core.IServices.Base;
using Core.Models;

namespace Core.IServices
{
    public interface IRankingServices : IBaseServices<RankingModel>
    {
        Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType);
        /// <summary>
        /// 更新排行榜数据
        /// </summary>
        Task UpdateRanking();
    }
}
