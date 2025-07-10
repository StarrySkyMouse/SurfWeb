namespace Repositories
{
    public enum DbTypeEnum
    {
        Sqlite = 0,
        MySQL = 1
    }
    public class DbConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbTypeEnum DbType { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public required string DbConnection { get; set; }
        /// <summary>
        /// 是否开启数据生成
        /// </summary>
        public bool IsDataCreate { get; set; }
    }
}
