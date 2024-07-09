using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace eShop.TestContainers.Tests;

public sealed class ElasticsearchContainerTest : IntegrationTestWebApplicationFactory
{
    public ElasticsearchContainerTest() : base(useElastic: true)
    {
    }

    [Fact]
    public async Task ReadFromElasticsearch()
    {
        var settings = new ElasticsearchClientSettings(new Uri(_elasticSearchContainer!.GetConnectionString()));
        settings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);

        var client = new ElasticsearchClient(settings);

        var stats = await client.PingAsync();

        Assert.True(stats.IsValidResponse);
    }
    [Fact]
    public async Task WriteToElasticsearch()
    {
        var settings = new ElasticsearchClientSettings(new Uri(_elasticSearchContainer!.GetConnectionString()));
        settings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);

        var client = new ElasticsearchClient(settings);

        var doc = new MyDoc
        {
            Id = 1,
            User = "flobernd",
            Message = "Trying out the client, so far so good?"
        };

        IndexResponse writeResponse = await client.IndexAsync(document: doc, index: "my_index");
        GetResponse<MyDoc> readResponse = await client.GetAsync<MyDoc>(doc.Id, idx => idx.Index("my_index"));

        Assert.NotNull(writeResponse);
        Assert.True(writeResponse.IsValidResponse);
        Assert.NotNull(readResponse);
        Assert.True(readResponse.IsValidResponse);
        Assert.Equal(doc.Message, readResponse.Source!.Message);
    }

    private class MyDoc
    {
        public int Id { get; set; }
        public string User { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}

