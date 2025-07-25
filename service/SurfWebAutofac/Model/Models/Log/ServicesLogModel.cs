using Model.Models.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.Log
{
    /// <summary>
    /// 服务层日志
    /// </summary>
    public class ServicesLogModel : BaseEntity
    {
        [SugarColumn(ColumnDescription = "内容", IsNullable = true)]
        public string Message { get; set; }
    }
}
