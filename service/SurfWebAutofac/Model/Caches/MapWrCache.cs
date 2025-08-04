using Model.Models.Main;

namespace Model.Caches;

public class MapWrCache
{
    /// <summary>
    ///     地图id
    /// </summary>
    public long MapId { get; set; }

    /// <summary>
    ///     地图名称
    /// </summary>
    public string? MapName { get; set; }

    /// <summary>
    ///     难度
    /// </summary>
    public string Difficulty { get; set; }

    /// <summary>
    ///     地图图片
    /// </summary>
    public string Img { get; set; }

    /// <summary>
    ///     玩家id
    /// </summary>
    public long PlayerId { get; set; }

    /// <summary>
    ///     玩家名
    /// </summary>
    public string PlayerName { get; set; }

    /// <summary>
    ///     时间
    /// </summary>
    public float Time { get; set; }

    /// <summary>
    ///     日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    ///     通关类型(0-主线，1-奖励，2-阶段)
    /// </summary>
    public RecordTypeEnum Type { get; set; }

    /// <summary>
    ///     阶段
    /// </summary>
    public int? Stage { get; set; }
}