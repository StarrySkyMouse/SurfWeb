namespace Model.Dtos.Maps;

public class MapTop100Dto
{
    /// <summary>
    ///     排行
    /// </summary>
    public required int Ranking { get; set; }

    /// <summary>
    ///     玩家id
    /// </summary>
    public required string PlayerId { get; set; }

    /// <summary>
    ///     玩家名
    /// </summary>
    public required string PlayerName { get; set; }

    /// <summary>
    ///     阶段
    /// </summary>
    public int? Stage { get; set; }

    /// <summary>
    ///     通关时间
    /// </summary>
    public float Time { get; set; }

    /// <summary>
    ///     时间差距
    /// </summary>
    public float GapTime { get; set; }

    /// <summary>
    ///     日期
    /// </summary>
    public DateTime Date { get; set; }
}