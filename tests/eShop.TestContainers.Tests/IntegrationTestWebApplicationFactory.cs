using Microsoft.AspNetCore.Mvc.Testing;
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

    public Task InitializeAsync()
    {
        var tasks = new List<Task>();
        if (_dbContainer is not null)
            tasks.Add(_dbContainer.StartAsync());
        if (_cacheContainer is not null)
            tasks.Add(_cacheContainer.StartAsync());
        if (_elasticSearchContainer is not null)
            tasks.Add(_elasticSearchContainer.StartAsync());
        if (_mongoDbContainer is not null)
            tasks.Add(_mongoDbContainer.StartAsync());

        return Task.WhenAll(tasks);
    }
    public new Task DisposeAsync()
    {
        var tasks = new List<Task>();
        if (_dbContainer is not null)
            tasks.Add(_dbContainer.DisposeAsync().AsTask());
        if (_cacheContainer is not null)
            tasks.Add(_cacheContainer.DisposeAsync().AsTask());
        if (_elasticSearchContainer is not null)
            tasks.Add(_elasticSearchContainer.DisposeAsync().AsTask());
        if (_mongoDbContainer is not null)
            tasks.Add(_mongoDbContainer.DisposeAsync().AsTask());

        return Task.WhenAll(tasks);
    }
}
