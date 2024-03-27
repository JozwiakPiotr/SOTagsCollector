
using SOTagsCollector.API.Entities;

namespace SOTagsCollector.API.Services;

public class StackExchangeClient : IStackExchangeClient
{
    public Task<List<Tag>> GetTags(GetTagsRequest request)
    {
        throw new NotImplementedException();
    }
}
