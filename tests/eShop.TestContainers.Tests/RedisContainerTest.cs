using StackExchange.Redis;
using Testcontainers.Redis;

namespace eShop.TestContainers.Tests;

public class RedisContainerTest : IAsyncLifetime
{
    private readonly RedisContainer _cacheContainer = new RedisBuilder()
        .WithImage("redis:alpine")
        .Build();

    public async Task DisposeAsync()
    {
        await _cacheContainer.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        await _cacheContainer.StartAsync();
    }

    [Fact]
    public async Task WriteToRedisDatabase()
    {
        var connectionString = _cacheContainer.GetConnectionString();
        var connection = ConnectionMultiplexer.Connect(connectionString);
        var db = connection.GetDatabase();

        await db.StringSetAsync("key", "value");

        var actual = await db.StringGetAsync("key");
        Assert.Equal("value", actual);
    }
}
