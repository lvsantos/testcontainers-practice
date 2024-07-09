using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace eShop.TestContainers.Tests;

public abstract class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected MsSqlContainer _dbContainer;
    protected RedisContainer _cacheContainer;

    public IntegrationTestWebApplicationFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server")
            .Build();
        _cacheContainer = new RedisBuilder()
            .WithImage("redis:alpine")
            .Build();
    }

    public Task InitializeAsync()
    {
        var tasks = new List<Task>
        {
            _cacheContainer.StartAsync(),
            _dbContainer.StartAsync()
        };
        return Task.WhenAll(tasks);
    }
    public new Task DisposeAsync()
    {
        var tasks = new List<Task>
        {
            _cacheContainer.DisposeAsync().AsTask(),
            _dbContainer.DisposeAsync().AsTask()
        };
        return Task.WhenAll(tasks);
    }
}
