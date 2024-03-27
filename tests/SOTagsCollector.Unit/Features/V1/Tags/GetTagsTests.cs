using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SOTagsCollector.API.Features.V1.Tags;

namespace SOTagsCollector.Unit.Features.V1.Tags;

public class GetTagsTests
{
    Fixture _fixture = FixtureFactory.Create();
    
    [Theory]
    [InlineData("desc","name")]
    [InlineData("asc","count")]
    public async Task HandleAsync_WhenValidQueryParameters_ShouldReturnStatusOk(string order, string sort)
    {
        var request = _fixture.Build<GetTagsRequest>()
            .With(x => x.Order, order)
            .With(x => x.Sort, sort).Create();
        var sut = _fixture.Create<GetTagsEndpoint>();

        var result = await sut.HandleAsync(request);

        var typedResult = Assert.IsType<Ok<GetTagsResponse>>(result);
        Assert.NotNull(typedResult.Value);
        Assert.NotNull(typedResult.Value!.Tags);
    }

    [Theory]
    [InlineData("a", "a")]
    public async Task HandleAsync_WhenInvalidQueryParameters_ShloudReturnStatusNotFound(string order, string sort)
    {
        var request = _fixture.Build<GetTagsRequest>()
            .With(x => x.Order, order)
            .With(x => x.Sort, sort).Create();
        var sut = _fixture.Create<GetTagsEndpoint>();

        var result = await sut.HandleAsync(request);

        Assert.IsType<NotFound>(result);
    }
}