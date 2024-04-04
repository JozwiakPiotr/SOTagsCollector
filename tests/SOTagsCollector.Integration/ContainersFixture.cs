using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;
using SOTagsCollector.API.Persistence;
using SOTagsCollector.Integration.Tools;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using WireMock.Server;

namespace SOTagsCollector.Integration;

[CollectionDefinition(nameof(ContainersFixture))]
public class ContainersCollection : ICollectionFixture<ContainersFixture>{}
public class ContainersFixture : IAsyncLifetime
{
    public readonly MsSqlContainer MsSql = new MsSqlBuilder().Build();
    public readonly RabbitMqContainer RabbitMq = new RabbitMqBuilder().Build();
    public readonly WireMockServer SEApi = WireMockServer.Start();
    public Respawner Respawner { get; private set; }
    public TagsDbContextFactory TagsDbContextFactory { get; private set; }

    public async Task InitializeAsync()
    {
        await MsSql.StartAsync();
        TagsDbContextFactory = new TagsDbContextFactory(MsSql.GetConnectionString());
        await MigrateDb();
        await InitRespawner();
        await RabbitMq.StartAsync();
    }

    private async Task MigrateDb()
    {
        await TagsDbContextFactory.Create()
            .Database.MigrateAsync();
    }

    private async Task InitRespawner()
    {
        Respawner = await Respawner.CreateAsync(
            MsSql.GetConnectionString(),
            new RespawnerOptions
            {
                TablesToIgnore = [new Table("__EFMigrationsHistory")]
            });
    }
    
    public async Task DisposeAsync()
    {
        SEApi.Stop();
        SEApi.Dispose();
        await MsSql.DisposeAsync();
        await RabbitMq.DisposeAsync();
    }
}