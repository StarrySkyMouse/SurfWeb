using IServices.Base;
using Model.Dtos.Rankings;
using Model.Models.Main;

namespace IServices.Main;

public interface IRankingServices : IBaseServices<RankingModel>
{
    Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType);

    /// <summary>
    ///     更新排行榜数据
    /// </summary>
    Task UpdateRanking();
}