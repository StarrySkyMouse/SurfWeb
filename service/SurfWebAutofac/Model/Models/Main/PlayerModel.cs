using Common.SqlSugar.BASE;
using SqlSugar;

namespace Model.Models.Main;

/// <summary>
///     玩家
/// </summary>
[SugarTable("Player")]
public class PlayerModel : BaseEntity
{
    /// <summary>
    ///     腐竹玩家id
    /// </summary>
    [SugarColumn(ColumnDescription = "腐竹玩家id")]
    public int Auth { get; set; }

    /// <summary>
    ///     玩家名
    /// </summary>
    [SugarColumn(ColumnDescription = "玩家名", IsNullable = true, Length = 128)]
    public string Name { get; set; }

    /// <summary>
    ///     积分
    /// </summary>
    [SugarColumn(ColumnDescription = "积分")]
    public decimal Integral { get; set; }

    /// <summary>
    ///     地图完成数
    /// </summary>
    [SugarColumn(ColumnDescription = "地图完成数")]
    public int SucceesNumber { get; set; }

    /// <summary>
    ///     主线wr数
    /// </summary>
    [SugarColumn(ColumnDescription = "主线wr数")]
    public int WRNumber { get; set; }

    /// <summary>
    ///     奖励wr数
    /// </summary>
    [SugarColumn(ColumnDescription = "奖励wr数")]
    public int BWRNumber { get; set; }

    /// <summary>
    ///     阶段wr数
    /// </summary>
    [SugarColumn(ColumnDescription = "阶段wr数")]
    public int SWRNumber { get; set; }
}