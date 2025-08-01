using Model.Entitys;

namespace Model.Dtos.Rankings;

public class RankingDto
{
    /// <summary>
    ///     排名类型(积分)
    /// </summary>
    public RankingTypeEnum Type { get; set; }

    /// <summary>
    ///     名次
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    ///     玩家id
    /// </summary>
    public required string PlayerId { get; set; }

    /// <summary>
    ///     玩家名
    /// </summary>
    public required string PlayerName { get; set; }

    /// <summary>
    ///     数值
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    ///     进度
    /// </summary>
    public string Progress { get; set; }
}