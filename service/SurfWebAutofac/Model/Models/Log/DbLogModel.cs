﻿using Model.Models.Base;
using SqlSugar;

namespace Model.Models.Log;

/// <summary>
///     数据库日志
/// </summary>
public class DbLogModel : BaseEntity
{
    [SugarColumn(ColumnDescription = "内容", IsNullable = true)]
    public string Message { get; set; }
}