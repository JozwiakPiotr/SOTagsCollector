using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SOTagsCollector.API.Features.Tags.UpdateTags;
using SOTagsCollector.API.Services;
using SOTagsCollector.Tools;
using SOTagsCollector.Unit.Tools;

namespace SOTagsCollector.Unit.Features.Tags;

public class UpdateTagsEndpointTests
{
    private Fixture _fixture = FixtureFactory.Create();

    [Theory, AutoMoqData]
    public async Task HandleAsync_Always_ReturnsOk(
        UpdateTagsRequest request,
        UpdateTagsRequestHandler sut)
    {
        var result = await sut.HandleAsync(request);

        Assert.IsType<Ok>(result);
    }

    [Theory, AutoMoqData]
    public async Task HandleAsync_Always_SendsCommand(
        [Frozen] Mock<ITagService> busMock,
        UpdateTagsRequest request,
        UpdateTagsRequestHandler sut)
    {
        _ = await sut.HandleAsync(request);

        busMock.Verify(x => x.UpdateAll(), Times.Once);
    }
}