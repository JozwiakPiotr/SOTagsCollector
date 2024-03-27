using SOTagsCollector.API.Entities;

namespace SOTagsCollector.API.Services;

public interface IStackExchangeClient
{
    Task<List<Tag>> GetTags(GetTagsRequest request);
}
