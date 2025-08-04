using Newtonsoft.Json;

namespace Common.Logger.Dto;

public enum ServiceLoggerDtoType
{
    Info,
    Error
}

public class ServiceLoggerDto
{
    public string Id { get; set; }

    /// <summary>
    ///     类型
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    ///     方法名称
    /// </summary>
    public string MethodName { get; set; }

    /// <summary>
    ///     参数
    /// </summary>
    public string Arguments { get; set; }

    /// <summary>
    ///     返回结果
    /// </summary>
    public object ReturnValue { get; set; }

    /// <summary>
    ///     用时
    /// </summary>
    public string TimeSpan { get; set; }

    /// <summary>
    ///     错误信息
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    ///     信息日志
    /// </summary>
    public static string InfoToString(string id, ServiceLoggerDtoType type, string methodName, string arguments,
        dynamic returnValue, long timeSpan)
    {
        return JsonConvert.SerializeObject(new ServiceLoggerDto
        {
            Id = id,
            Type = type.ToString(),
            MethodName = methodName,
            Arguments = arguments,
            ReturnValue = returnValue,
            TimeSpan = timeSpan + "ms"
        });
    }

    //错误日志
    public static string ErrorToString(string id, ServiceLoggerDtoType type, string methodName, string arguments,
        object returnValue, long timeSpan, string error)
    {
        return JsonConvert.SerializeObject(new ServiceLoggerDto
        {
            Id = id,
            Type = type.ToString(),
            MethodName = methodName,
            Arguments = arguments,
            ReturnValue = returnValue,
            TimeSpan = timeSpan + "ms",
            Error = error
        });
    }
}