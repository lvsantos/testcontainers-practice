using MongoDB.Bson;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace eShop.TestContainers.Tests;

public sealed class MongoContainerTest : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .Build();

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
    }

    [Fact]
    public async Task ReadFromMongoDbDatabase()
    {
        var client = new MongoClient(_mongoDbContainer!.GetConnectionString());

        using var databases = await client.ListDatabasesAsync();

        Assert.True(await databases.AnyAsync());
    }
    [Fact]
    public async Task WriteToMongoDbDatabase()
    {
        var client = new MongoClient(_mongoDbContainer!.GetConnectionString());
        var database = client.GetDatabase("test");
        var collection = database.GetCollection<BsonDocument>("test");

        var document = new BsonDocument
        {
            { "name", "MongoDB" },
            { "type", "Database" },
            { "count", 1 },
            { "info", new BsonDocument
                {
                    { "x", 203 },
                    { "y", 102 }
                }
            }
        };

        await collection.InsertOneAsync(document);

        var documents = await collection.Find(new BsonDocument()).ToListAsync();

        Assert.True(documents.Any());
        Assert.Contains(documents, d => d["name"] == "MongoDB");
        Assert.Contains(documents, d => d["type"] == "Database");
        Assert.Contains(documents, d => d["count"] == 1);
        Assert.Contains(documents, d => d["info"]["x"] == 203);
        Assert.Contains(documents, d => d["info"]["y"] == 102);
    }
}

