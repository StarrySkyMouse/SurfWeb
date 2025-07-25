using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Quartz
{
    public class JobConfig
    {
        /// <summary>
        /// 同步任务执行
        /// </summary>
        public int SequenceJobMinute { get; set; }
        public int ServerInfoCacheJobSecond { get; set; }
    }
}
