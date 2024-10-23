using System.Data;
using Microsoft.Data.SqlClient;
using RPSSL.Application.Abstractions.Data;

namespace RPSSL.Infrastructure.Data;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        
        connection.Open();

        return connection;
    }
}
