using Common.SqlSugar.BASE;
using SqlSugar;

namespace Model.Models.Main;

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

public enum RankingTypeEnum
{
    /// <summary>
    /// 积分
    /// </summary>
    Integral,

    /// <summary>
    /// 主线
    /// </summary>
    MainWR,

    /// <summary>
    /// 奖励
    /// </summary>
    BountyWR,

    /// <summary>
    /// 阶段
    /// </summary>
    StageWR
}

/// <summary>
///     最新记录
/// </summary>
[SugarTable("NewRecord")]
public class NewRecordModel : BaseEntity
{
    /// <summary>
    ///     玩家id
    /// </summary>
    [SugarColumn(ColumnDescription = "玩家id", IsNullable = true, Length = 64)]
    public long PlayerId { get; set; }

    /// <summary>
    ///     玩家名
    /// </summary>
    [SugarColumn(ColumnDescription = "玩家名", IsNullable = true, Length = 128)]
    public string PlayerName { get; set; }

    /// <summary>
    ///     地图id
    /// </summary>
    [SugarColumn(ColumnDescription = "地图id", IsNullable = true, Length = 64)]
    public long MapId { get; set; }

    /// <summary>
    ///     地图名称
    /// </summary>
    [SugarColumn(ColumnDescription = "地图名称", IsNullable = true, Length = 128)]
    public string MapName { get; set; }

    /// <summary>
    ///     通关类型(0-主线，1-奖励，2-阶段)
    /// </summary>
    [SugarColumn(ColumnDescription = "通关类型(0-主线，1-奖励，2-阶段)", Length = 1)]
    public RecordTypeEnum Type { get; set; }

    /// <summary>
    ///     说明难度或阶段
    /// </summary>
    [SugarColumn(ColumnDescription = "说明难度或阶段", IsNullable = true, Length = 64)]
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