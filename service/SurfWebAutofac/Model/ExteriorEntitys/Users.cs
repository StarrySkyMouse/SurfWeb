namespace Model.ExteriorEntitys
{
    /// <summary>
    /// 玩家
    /// </summary>
    public class Users
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int auth { get; set; }
        /// <summary>
        /// 玩家名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 玩家积分
        /// </summary>
        public decimal points { get; set; }
    }
}
