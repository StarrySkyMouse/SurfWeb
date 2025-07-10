
namespace Common.Caches;

/// <summary>
/// 缓存抽象接口,基于IDistributedCache封装
/// </summary>
public interface ICaching
{
    bool Exists(string cacheKey);
    T? Get<T>(string cacheKey) where T : class;
    void Remove(string key);
    void Set<T>(string cacheKey, T value);
    /// <summary>
    /// 获取缓存或执行函数并缓存结果
    /// </summary>
    T GetOrFunction<T>(string cacheKey, Func<T> func) where T : class;
}