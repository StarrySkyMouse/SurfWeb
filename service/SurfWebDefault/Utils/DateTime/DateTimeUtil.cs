namespace Utils.DateTime
{
    public class DateTimeUtil
    {
        private static readonly TimeZoneInfo ChinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
        /// <summary>
        /// 将Unix时间戳转换为北京时间
        /// </summary>
        public static System.DateTime FromUnixTimestamp(long unixTimeStamp)
        {
            var utcTime = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, ChinaTimeZone);
        }
        /// <summary>
        /// 将DateTime时间戳（本地或UTC）转换为北京时间的Unix
        /// </summary>
        public static long ToUnixTimestamp(System.DateTime? dateTime)
        {
            if (dateTime == null) return 0;
            var beijingTime = TimeZoneInfo.ConvertTime(((System.DateTime)dateTime), ChinaTimeZone);
            return new DateTimeOffset(beijingTime).ToUnixTimeSeconds();
        }
    }
}