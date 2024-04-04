using MassTransit;
using SOTagsCollector.API.Handlers;

namespace SOTagsCollector.API.Services;

public class TagService : ITagService
{
    private const int TagsToFetchCount = 1000;
    private const int BatchSize = 100;
    private readonly IBus _bus;

    public TagService(IBus bus)
    {
        _bus = bus;
    }

    public async Task UpdateAll()
    {
        for (var i = 1; i <= TagsToFetchCount / BatchSize; i++)
        {
            await _bus.Publish(new UpdateTagsCommand(
                Page: i, Count: BatchSize));
        }
    }
}