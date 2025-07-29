using Common.SqlSugar.BASE;
using SqlSugar;

namespace Model.Models.Log;

/// <summary>
///     服务层日志
/// </summary>
[SugarTable("ServicesLog")]
public class ServicesLogModel : BaseEntity
{
    [SugarColumn(ColumnDescription = "内容", IsNullable = true)]
    public string Message { get; set; }
}