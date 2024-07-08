using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;

namespace eShop.TestContainers.Tests;

public abstract class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected MsSqlContainer _dbContainer;

    public IntegrationTestWebApplicationFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .Build();
    }

    public Task InitializeAsync() => _dbContainer.StartAsync();
    Task IAsyncLifetime.DisposeAsync() => _dbContainer.DisposeAsync().AsTask();
}
