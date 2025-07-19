namespace Configurations.Middlewares;

/// <summary>
///     自定义异常
/// </summary>
[Serializable]
public class CustomException : Exception
{
    public CustomException(string message, int errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public int ErrorCode { get; set; }

    /// <summary>
    ///     抛出自定义异常
    /// </summary>
    public static void Throw(string message)
    {
        throw new CustomException(message, 50000);
    }
}