using StackExchange.Redis;

namespace eShop.TestContainers.Tests;

public class RedisContainerTest : IntegrationTestWebApplicationFactory
{
    public RedisContainerTest() : base(useCache: true)
    {
    }

    [Fact]
    public async Task WriteToRedisDatabase()
    {
        var connectionString = _cacheContainer!.GetConnectionString();
        var connection = ConnectionMultiplexer.Connect(connectionString);
        var db = connection.GetDatabase();

        await db.StringSetAsync("key", "value");

        var actual = await db.StringGetAsync("key");
        Assert.Equal("value", actual);
    }
}
