using Microsoft.Data.SqlClient;

namespace eShop.TestContainers.Tests;

public class MsSqlServerContainerTest : IntegrationTestWebApplicationFactory
{
    [Fact]
    public async Task ReadFromMsSqlDatabase()
    {
        await using var connection = new SqlConnection(_dbContainer.GetConnectionString());
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1;";

        var actual = await command.ExecuteScalarAsync() as int?;
        Assert.Equal(1, actual.GetValueOrDefault());
    }
}
