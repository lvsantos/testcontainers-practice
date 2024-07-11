using Microsoft.Data.SqlClient;
using Npgsql;
using System.Data.Common;

namespace eShop.TestContainers.Api;

public class DbConnectionProvider
{
    private readonly string _connectionString;

    public DbConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public virtual DbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
