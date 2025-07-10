namespace Utils.DateTime
{
    public class DateTimeUtil
    {
        /// <summary>
        /// 获取当前时间的Unix时间戳
        /// </summary>
        public static long GetCurrentUnixTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        /// <summary>
        /// 将Unix时间戳转换为DateTime
        /// </summary>
        public static System.DateTime FromUnixTimestamp(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
        }
        /// <summary>
        /// 将DateTime时间戳转换为Unix
        /// </summary>
        public static long ToUnixTimestamp(System.DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return 0;
            }
            return new DateTimeOffset(((System.DateTime)dateTime).ToUniversalTime()).ToUnixTimeSeconds();
        }
    }
}
