using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.IdWorker
{
    public static class SnowflakeUtil
    {
        private static readonly Snowflake.Core.IdWorker _worker = new(1, 1);
        /// <summary>
        ///     获取下一个唯一ID
        /// </summary>
        /// <returns></returns>
        public static long NextId()
        {
            return _worker.NextId();
        }
        /// <summary>
        ///     获取下一个唯一ID字符串
        /// </summary>
        /// <returns></returns>
        public static string NextIdString()
        {
            return _worker.NextId().ToString();
        }
    }
}
