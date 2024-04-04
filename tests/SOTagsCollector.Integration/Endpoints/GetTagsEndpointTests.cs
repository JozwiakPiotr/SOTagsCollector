using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SOTagsCollector.API.Clients;
using SOTagsCollector.API.Features.Tags.GetTags;
using SOTagsCollector.Tools;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using static SOTagsCollector.Integration.Tools.TestContext;

namespace SOTagsCollector.Integration.Endpoints;

[Collection(nameof(ContainersFixture))]
public class GetTagsEndpointTests : IDisposable
{
    private readonly Fixture _fixture = FixtureFactory.Create();
    private readonly ContainersFixture _containersFixture;
    private readonly WebApplicationFactory<Program> _testWebAppFactoy;

    public GetTagsEndpointTests(ContainersFixture containersFixture)
    {
        _containersFixture = containersFixture;
        _testWebAppFactoy = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder.AddInMemoryCollection(
                        new Dictionary<string, string>
                        {
                            ["ConnectionStrings:tagsdb"] = _containersFixture.MsSql.GetConnectionString(),
                            ["ConnectionStrings:rabbitMq"]=_containersFixture.RabbitMq.GetConnectionString()
                        }!);
                })
                .ConfigureTestServices(services =>
                {
                    services.Configure<StackExchangeConfig>(cfg =>
                    {
                        var baseAddress = new Uri(_containersFixture.SEApi.Urls[0]);
                        cfg.BaseUri = baseAddress;
                    });
                })
        );
    }

    [Fact]
    public async Task Get_WhenValidRequest_ReturnsPaginatedResult()
    {
        _containersFixture.SEApi.Given(
                Request.Create().WithPath("/tags").UsingGet())
            .AtPriority(2)
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "text/plain")
                    .WithBodyFromFile(Path.Combine(TestDataDirectory,
                        "empty_page_response.json"))
            );
        _containersFixture.SEApi.Given(
            Request.Create().WithPath("/tags").WithParam("page","1").UsingGet())
            .AtPriority(1)
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "text/plain")
                    .WithBodyFromFile(Path.Combine(TestDataDirectory,
                        "top20by_popular_desc_response.json"))
                
            );
        const int expectedPageSize = 5;
        var client = _testWebAppFactoy.CreateClient();
        Thread.Sleep(TimeSpan.FromSeconds(5));
        
        var response = await client.GetAsync($"api/tags?page=1&pageSize={expectedPageSize}&sort=count&order=desc");
        
        Assert.True(response.IsSuccessStatusCode);
        var content = (await response.Content.ReadFromJsonAsync<GetTagsResponse>())!;
        Assert.Equal(expectedPageSize, content.Tags.Count);
        content.Tags.Should().BeInDescendingOrder(x => x.Count);
    }

    public void Dispose()
    {
        _containersFixture.Respawner.ResetAsync(_containersFixture.MsSql.GetConnectionString()).GetAwaiter().GetResult();
    }
}