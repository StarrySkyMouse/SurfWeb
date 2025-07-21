using Model.Models.Base;
using SqlSugar;

namespace Model.Models;

/// <summary>
///     地图
/// </summary>
public class MapModel : BaseEntity
{
    [SugarColumn(ColumnDescription = "名称", IsNullable = false, Length = 128)]
    public string Name { get; set; }

    /// <summary>
    ///     难度
    /// </summary>
    [SugarColumn(ColumnDescription = "难度", IsNullable = false, Length = 16)]
    public string Difficulty { get; set; }

    /// <summary>
    ///     地图图片
    /// </summary>
    [SugarColumn(ColumnDescription = "地图图片", IsNullable = false)]
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