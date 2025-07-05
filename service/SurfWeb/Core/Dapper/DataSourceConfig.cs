namespace Core.Dapper
{
    public class DataSourceConfig
    {
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsDataSync { get; set; }
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public required string DbConnection { get; set; }
    }
}
