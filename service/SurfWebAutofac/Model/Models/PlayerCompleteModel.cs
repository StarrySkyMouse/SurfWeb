using Model.Models.Base;
using SqlSugar;

namespace Model.Models;

/// <summary>
///     玩家通关
/// </summary>
public class PlayerCompleteModel : BaseEntity
{
    /// <summary>
    ///     腐竹玩家id
    /// </summary>
    [SugarColumn(ColumnDescription = "腐竹玩家id")]
    public int Auth { get; set; }

    /// <summary>
    ///     玩家id
    /// </summary>
    [SugarColumn(ColumnDescription = "玩家id", Length = 64)]
    public long PlayerId { get; set; }

    /// <summary>
    ///     玩家名
    /// </summary>
    [SugarColumn(ColumnDescription = "玩家名", Length = 128)]
    public string? PlayerName { get; set; }

    /// <summary>
    ///     地图id
    /// </summary>
    [SugarColumn(ColumnDescription = "地图id", Length = 64)]
    public long MapId { get; set; }

    /// <summary>
    ///     地图名称
    /// </summary>
    [SugarColumn(ColumnDescription = "地图名称", Length = 128)]
    public string? MapName { get; set; }

    /// <summary>
    ///     通关类型(0-主线，1-奖励，2-阶段)
    /// </summary>
    [SugarColumn(ColumnDescription = "通关类型(0-主线，1-奖励，2-阶段)", Length = 1)]
    public RecordTypeEnum Type { get; set; }

    /// <summary>
    ///     阶段
    /// </summary>
    [SugarColumn(ColumnDescription = "阶段", Length = 2)]
    public int? Stage { get; set; }

    /// <summary>
    ///     通关时间
    /// </summary>
    [SugarColumn(ColumnDescription = "通关时间")]
    public float Time { get; set; }

    /// <summary>
    ///     日期
    /// </summary>
    [SugarColumn(ColumnDescription = "日期")]
    public DateTime Date { get; set; }

    /// <summary>
    ///     是否隐藏
    /// </summary>
    [SugarColumn(ColumnDescription = "是否隐藏")]
    public bool Hide { get; set; }
}