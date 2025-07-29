using System.Data;
using Dapper;
using MySqlConnector;

namespace Common.Dapper;

public class MySqlHelp : ISqlHelp
{
    private readonly string _dbConnection;

    public MySqlHelp(string dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<T>(sql, param);
    }

    public async Task<IEnumerable<T>> QueryPageAsync<T>(string sql, int pageIndex, int pageSize,
        object? param = null)
    {
        // 计算偏移量
        var offset = (pageIndex - 1) * pageSize;
        // 拼接分页SQL
        var pageSql = $"{sql} LIMIT ? OFFSET ?";
        // 合并参数
        var parameters = param == null ? new DynamicParameters() : new DynamicParameters(param);
        parameters.Add("PageSize", pageSize);
        parameters.Add("Offset", offset);
        using var connection = CreateConnection();
        return await connection.QueryAsync<T>(pageSql, parameters);
    }

    private IDbConnection CreateConnection()
    {
        return new MySqlConnection(_dbConnection);
    }
}