using SOTagsCollector.API.Persistance;
using SOTagsCollector.API.Services;

namespace SOTagsCollector.API.Handlers;

public class UpdateTagsHandler
{
    private readonly IStackExchangeClient _stackExchangeClient;
    private const int BatchSize = 100;
    public UpdateTagsHandler(
        IStackExchangeClient stackExchangeClient,
        TagsDb tagsDb)
    {
        _stackExchangeClient = stackExchangeClient;
    }

    public Task HandleAsync(UpdateTagsCommand _)
    {
        throw new NotImplementedException();
    }
}

public record UpdateTagsCommand();