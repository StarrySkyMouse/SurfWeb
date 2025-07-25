using Model.Models.Base;
using SqlSugar;

namespace Model.Models.Log;

/// <summary>
///     服务层日志
/// </summary>
public class ServicesLogModel : BaseEntity
{
    [SugarColumn(ColumnDescription = "内容", IsNullable = true)]
    public string Message { get; set; }
}