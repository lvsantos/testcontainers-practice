﻿using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Elasticsearch;
using Testcontainers.MongoDb;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace eShop.TestContainers.Tests;

public abstract class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected readonly MsSqlContainer _dbContainer;
    protected readonly RedisContainer _cacheContainer;
    protected readonly ElasticsearchContainer _elasticSearchContainer;
    protected readonly MongoDbContainer _mongoDbContainer;

    public IntegrationTestWebApplicationFactory()
    {
        _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server")
            .Build();
        _cacheContainer = new RedisBuilder()
            .WithImage("redis:alpine")
            .Build();
        _elasticSearchContainer = new ElasticsearchBuilder()
            .Build();
        _mongoDbContainer = new MongoDbBuilder()
            .Build();
    }

    public Task InitializeAsync()
    {
        var tasks = new List<Task>
        {
            _cacheContainer.StartAsync(),
            _dbContainer.StartAsync(),
            _elasticSearchContainer.StartAsync(),
            _mongoDbContainer.StartAsync()
        };
        return Task.WhenAll(tasks);
    }
    public new Task DisposeAsync()
    {
        var tasks = new List<Task>
        {
            _cacheContainer.DisposeAsync().AsTask(),
            _dbContainer.DisposeAsync().AsTask(),
            _elasticSearchContainer.DisposeAsync().AsTask(),
            _mongoDbContainer.DisposeAsync().AsTask()
        };
        return Task.WhenAll(tasks);
    }
}
