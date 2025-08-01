using Model.Models.Main;

namespace Model.Dtos.Rankings;

public class RankingDto
{
    /// <summary>
    ///     排名类型(积分)
    /// </summary>
    public RankingTypeEnum Type { get; set; }

    /// <summary>
    ///     玩家id
    /// </summary>
    public long PlayerId { get; set; }

    /// <summary>
    ///     玩家名
    /// </summary>
    public string PlayerName { get; set; }

    /// <summary>
    ///     数值
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    ///     进度
    /// </summary>
    public string Progress { get; set; }
}