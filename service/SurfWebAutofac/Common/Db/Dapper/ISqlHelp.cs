namespace Common.Db.Dapper;

public interface ISqlHelp
{
    /// <summary>
    ///     SQL查询
    /// </summary>
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);

    /// <summary>
    ///     分页查询
    /// </summary>
    Task<IEnumerable<T>> QueryPageAsync<T>(string sql, int pageIndex, int pageSize, object? param = null);
}