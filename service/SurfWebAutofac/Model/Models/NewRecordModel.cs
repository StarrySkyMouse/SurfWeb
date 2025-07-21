using Model.Models.Base;
using SqlSugar;

namespace Model.Models;

/// <summary>
///     纪录类型
/// </summary>
public enum RecordTypeEnum
{
    /// <summary>
    ///     主线
    /// </summary>
    Main,

    /// <summary>
    ///     奖励
    /// </summary>
    Bounty,

    /// <summary>
    ///     阶段
    /// </summary>
    Stage
}

/// <summary>
///     最新记录
/// </summary>
public class NewRecordModel : BaseEntity
{
    /// <summary>
    ///     玩家id
    /// </summary>
    [SugarColumn(ColumnDescription = "玩家id", IsNullable = false, Length = 64)]
    public long PlayerId { get; set; }

    /// <summary>
    ///     玩家名
    /// </summary>
    [SugarColumn(ColumnDescription = "玩家名", IsNullable = false, Length = 128)]
    public string PlayerName { get; set; }

    /// <summary>
    ///     地图id
    /// </summary>
    [SugarColumn(ColumnDescription = "地图id", IsNullable = false, Length = 64)]
    public long MapId { get; set; }

    /// <summary>
    ///     地图名称
    /// </summary>
    [SugarColumn(ColumnDescription = "地图名称", IsNullable = false, Length = 128)]
    public string MapName { get; set; }

    /// <summary>
    ///     通关类型(0-主线，1-奖励，2-阶段)
    /// </summary>
    [SugarColumn(ColumnDescription = "通关类型(0-主线，1-奖励，2-阶段)", IsNullable = false, Length = 1)]
    public RecordTypeEnum Type { get; set; }

    /// <summary>
    ///     说明难度或阶段
    /// </summary>
    [SugarColumn(ColumnDescription = "说明难度或阶段", IsNullable = false, Length = 64)]
    public string Notes { get; set; }

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
}