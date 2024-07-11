/*using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Elasticsearch;
using Testcontainers.MongoDb;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace eShop.TestContainers.Tests;

public abstract class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected readonly MsSqlContainer? _dbContainer;
    protected readonly RedisContainer? _cacheContainer;
    protected readonly ElasticsearchContainer? _elasticSearchContainer;
    protected readonly MongoDbContainer? _mongoDbContainer;

    public IntegrationTestWebApplicationFactory(
        bool useDatabase = false,
        bool useCache = false,
        bool useElastic = false,
        bool useMongo = false)
    {
        if (useDatabase)
        {
            _dbContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server")
                .Build(); 
        }
        if (useCache)
        {
            _cacheContainer = new RedisBuilder()
                .WithImage("redis:alpine")
                .Build(); 
        }
        if (useElastic)
        {
            _elasticSearchContainer = new ElasticsearchBuilder()
                .Build(); 
        }
        if (useMongo)
        {
            _mongoDbContainer = new MongoDbBuilder()
                .Build(); 
        }
    }

    public async Task InitializeAsync()
    {
        if (_dbContainer is not null)
            await _dbContainer.StartAsync();
        if (_cacheContainer is not null)
            await _cacheContainer.StartAsync();
        if (_elasticSearchContainer is not null)
            await _elasticSearchContainer.StartAsync();
        if (_mongoDbContainer is not null)
            await _mongoDbContainer.StartAsync();

        return;
    }
    public new async Task DisposeAsync()
    {
        if (_dbContainer is not null)
            await _dbContainer.StopAsync();
        if (_cacheContainer is not null)
            await _cacheContainer.StopAsync();
        if (_elasticSearchContainer is not null)
            await _elasticSearchContainer.StopAsync();
        if (_mongoDbContainer is not null)
            await _mongoDbContainer.StopAsync();

        return;
    }
}
*/