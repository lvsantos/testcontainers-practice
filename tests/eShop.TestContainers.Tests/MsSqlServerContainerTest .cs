﻿using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace eShop.TestContainers.Tests;

public class MsSqlServerContainerTest : IAsyncLifetime
{
    public readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .Build();

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }
}

public class SqlTest : IClassFixture<MsSqlServerContainerTest>
{
    private readonly MsSqlServerContainerTest _fixture;

    public SqlTest(MsSqlServerContainerTest fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ReadFromMsSqlDatabase()
    {
        await using var connection = new SqlConnection(_fixture._dbContainer.GetConnectionString());
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1;";

        var actual = await command.ExecuteScalarAsync() as int?;
        Assert.Equal(1, actual.GetValueOrDefault());
    }
    [Fact]
    public async Task WriteToMsSqlDatabase()
    {
        await using var connection = new SqlConnection(_fixture._dbContainer.GetConnectionString());
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
    [Fact]
    public async Task Check()
    {
        await using var connection = new SqlConnection(_fixture._dbContainer.GetConnectionString());
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