using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Respawn;
using SOTagsCollector.API.Persistence;
using SOTagsCollector.API.Entities;
using SOTagsCollector.API.Repositories;
using SOTagsCollector.Integration.Tools;
using SOTagsCollector.Tools;

namespace SOTagsCollector.Integration.Repositories;

[Collection(nameof(ContainersFixture))]
public class TagRepositoryTests : IDisposable
{
    private readonly ContainersFixture _containersFixture;
    private readonly TagsDb _helperDbContext;

    public TagRepositoryTests(ContainersFixture containersFixture)
    {
        _containersFixture = containersFixture;
        _helperDbContext = _containersFixture.TagsDbContextFactory.Create();
    }

    [Theory, MemberData(nameof(GetTags))]
    public async Task MergeAsync_Always_Merge(
        List<Tag> given, List<Tag> passed, List<Tag> expected)
    {
        await _helperDbContext.ExecuteAsync(async ctx => await ctx.AddRangeAsync(given));
        await using var testTagsDb = _containersFixture.TagsDbContextFactory.Create();
        var sut = new TagRepository(testTagsDb);

        await sut.MergeAsync(passed);

        var actual = await _helperDbContext.Tags.ToListAsync();
        Assert.Equal(expected.Count, actual.Count);
        Assert.Equivalent(expected, actual);
    }
    public static IEnumerable<object[]> GetTags()
    {
        yield return new object[]
        {
            new List<Tag> {},
            new List<Tag> { new("name", 1) },
            new List<Tag> { new("name", 1) }
        };
        yield return new object[]
        {
            new List<Tag> { new("a", 1), new("b", 1) },
            new List<Tag> { new("b", 2), new("c", 1) },
            new List<Tag> { new("a", 1), new("b", 2), new("c", 1) }
        };
    }

    public void Dispose()
    {
        _containersFixture.Respawner.ResetAsync(_containersFixture.MsSql.GetConnectionString()).GetAwaiter().GetResult();
        _helperDbContext.Dispose();
    }
}