namespace Common.Caches.Base;

public interface ICache
{
    /// <summary>
    ///     检查key是否存在
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="action">不存在的时候执行的方法</param>
    /// <returns></returns>
    bool Exists(string cacheKey, Action? action = null);

    /// <summary>
    ///     获取
    /// </summary>
    T? Get<T>(string cacheKey, T? defaultValue = default);

    /// <summary>
    ///     获取缓存或执行操作（缓存失效执行操作）
    /// </summary>
    public T? GetOrFunc<T>(string cacheKey, Func<T> func, int cacheTime = -1);

    /// <summary>
    ///     设置带过期时间的缓存
    /// </summary>
    void Set<T>(string cacheKey, T value, int cacheTime = -1);

    /// <summary>
    ///     删除
    /// </summary>
    /// <param name="key"></param>
    void Remove(string key);
}