using Common.Db.Base;
using SqlSugar;

namespace Model.Models.Log;

/// <summary>
///     数据库日志
/// </summary>
[SugarTable("DbLog")]
public class DbLogModel : BaseEntity
{
    [SugarColumn(ColumnDescription = "内容", IsNullable = true)]
    public string Message { get; set; }
}