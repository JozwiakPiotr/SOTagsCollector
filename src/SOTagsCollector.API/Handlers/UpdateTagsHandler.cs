using MassTransit;
using SOTagsCollector.API.Clients;
using SOTagsCollector.API.Repositories;
using SOTagsCollector.API.Services;

namespace SOTagsCollector.API.Handlers;

public class UpdateTagsHandler : IConsumer<UpdateTagsCommand>
{
    private readonly IStackExchangeClient _stackExchangeClient;
    private readonly ITagRepository _tagRepository;

    public UpdateTagsHandler(
        IStackExchangeClient stackExchangeClient,
        ITagRepository tagRepository)
    {
        _stackExchangeClient = stackExchangeClient;
        _tagRepository = tagRepository;
    }

    public async Task Consume(ConsumeContext<UpdateTagsCommand> context)
    {
        var request = CreateGetTagsRequest(context.Message);
        var tags = await _stackExchangeClient.GetTagsAsync(request);
        await _tagRepository.MergeAsync(tags);
    }

    private GetTagsRequest CreateGetTagsRequest(UpdateTagsCommand cmd)
    {
        return new GetTagsRequest(
            cmd.Page, cmd.Count, "desc", "popular", "stackoverflow");
    }
}

public record UpdateTagsCommand(int Page, int Count);