using Newtonsoft.Json;

namespace Common.Logger.Dto;

public enum DbLoggerDtoType
{
    Info,
    Error
}

public class DbLoggerDto
{
    public string Id { get; set; }

    /// <summary>
    ///     类型
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    ///     执行sql
    /// </summary>
    public string Sql { get; set; }

    /// <summary>
    ///     执行时间
    /// </summary>
    public string TimeSpan { get; set; }

    /// <summary>
    ///     错误信息
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    ///     信息日志
    /// </summary>
    public static string InfoToString(string id, DbLoggerDtoType type, string sql, TimeSpan timeSpan)
    {
        return JsonConvert.SerializeObject(new DbLoggerDto
        {
            Id = id,
            Type = type.ToString(),
            Sql = sql,
            TimeSpan = (int)timeSpan.TotalMilliseconds + "ms"
        });
    }

    //错误日志
    public static string ErrorToString(string id, DbLoggerDtoType type, string sql, TimeSpan timeSpan, string error)
    {
        return JsonConvert.SerializeObject(new DbLoggerDto
        {
            Id = id,
            Type = type.ToString(),
            Sql = sql,
            TimeSpan = (int)timeSpan.TotalMilliseconds + "ms",
            Error = error
        });
    }
}