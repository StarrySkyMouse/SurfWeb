using Model.Dtos.Rankings;
using Model.Entitys;
using Services.Base;

namespace Services.IServices;

public interface IRankingServices : IBaseServices<RankingModel>
{
    Task<List<RankingDto>> GetRankingList(RankingTypeEnum rankingType);

    /// <summary>
    ///     更新排行榜数据
    /// </summary>
    Task UpdateRanking();
}