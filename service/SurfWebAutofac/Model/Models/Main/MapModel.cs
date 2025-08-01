using Common.Db.Base;
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
    ///     积分
    /// </summary>
    Integral,

    /// <summary>
    ///     主线
    /// </summary>
    MainWR,

    /// <summary>
    ///     奖励
    /// </summary>
    BountyWR,

    /// <summary>
    ///     阶段
    /// </summary>
    StageWR
}
/// <summary>
///     地图
/// </summary>
[SugarTable("Map")]
public class MapModel : BaseEntity
{
    [SugarColumn(ColumnDescription = "名称", Length = 128)]
    public string Name { get; set; }

    /// <summary>
    ///     难度
    /// </summary>
    [SugarColumn(ColumnDescription = "难度", Length = 16)]
    public string Difficulty { get; set; }

    /// <summary>
    ///     地图图片
    /// </summary>
    [SugarColumn(ColumnDescription = "地图图片")]
    public string Img { get; set; }

    /// <summary>
    ///     完成人数
    /// </summary>
    [SugarColumn(ColumnDescription = "完成人数")]
    public int SurcessNumber { get; set; }

    /// <summary>
    ///     地图奖励关数量
    /// </summary>
    [SugarColumn(ColumnDescription = "地图奖励关数量")]
    public int BonusNumber { get; set; }

    /// <summary>
    ///     地图阶段数量
    /// </summary>
    [SugarColumn(ColumnDescription = "地图阶段数量")]
    public int StageNumber { get; set; }
}