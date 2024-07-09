﻿using Microsoft.Data.SqlClient;

namespace eShop.TestContainers.Tests;

public class MsSqlServerContainerTest : IntegrationTestWebApplicationFactory
{
    public MsSqlServerContainerTest() : base(useDatabase: true)
    {
    }

    [Fact]
    public async Task ReadFromMsSqlDatabase()
    {
        await using var connection = new SqlConnection(_dbContainer!.GetConnectionString());
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1;";

        var actual = await command.ExecuteScalarAsync() as int?;
        Assert.Equal(1, actual.GetValueOrDefault());
    }
    [Fact]
    public async Task WriteToMsSqlDatabase()
    {
        await using var connection = new SqlConnection(_dbContainer!.GetConnectionString());
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();

        command.CommandText = "CREATE TABLE TestTable (Id INT);";
        await command.ExecuteNonQueryAsync();

        command.CommandText = "INSERT INTO TestTable (Id) VALUES (10);";
        await command.ExecuteNonQueryAsync();

        command.CommandText = "SELECT Id FROM TestTable;";
        var actual = await command.ExecuteScalarAsync() as int?;

        Assert.Equal(10, actual);
    }
}
