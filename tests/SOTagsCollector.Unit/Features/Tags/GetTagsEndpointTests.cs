using AutoFixture;
using Microsoft.AspNetCore.Http.HttpResults;
using SOTagsCollector.API.Features.Tags.GetTags;
using SOTagsCollector.API.Persistence;
using SOTagsCollector.Tools;

namespace SOTagsCollector.Unit.Features.Tags;

public class GetTagsEndpointTests
{
    Fixture _fixture = FixtureFactory.Create();

    [Theory]
    [InlineData("a", "a")]
    public async Task HandleAsync_InvalidQueryParameters_ReturnsStatusNotFound(string order, string sort)
    {
        var request = _fixture.Build<GetTagsRequest>()
            .With(x => x.Order, order)
            .With(x => x.Sort, sort).Create();
        var sut = _fixture.Create<GetTagsHandler>();

        var result = await sut.HandleAsync(request);

        Assert.IsType<NotFound>(result);
    }
}