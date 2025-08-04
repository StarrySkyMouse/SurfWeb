using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace Common.Db.Base;

/// <summary>
///     基类
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    ///     主键（配置雪花算法生成的id）
    /// </summary>
    [SugarColumn(ColumnDescription = "主键", IsNullable = false, IsPrimaryKey = true)] //sql sugar的特性
    [MaxLength(64)] //ef core的特性
    public long Id { get; set; }

    /// <summary>
    ///     创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", IsNullable = false)]
    public DateTime CreateTime { get; set; }

    /// <summary>
    ///     修改时间
    /// </summary>
    [SugarColumn(ColumnDescription = "修改时间", IsNullable = false)]
    public DateTime UpDateTime { get; set; }

    /// <summary>
    ///     删除标志0-未删除，1-删除
    /// </summary>
    [SugarColumn(ColumnDescription = "修改时间", IsNullable = false, Length = 1)]
    [MaxLength(1)]
    public int IsDelete { get; set; }
}