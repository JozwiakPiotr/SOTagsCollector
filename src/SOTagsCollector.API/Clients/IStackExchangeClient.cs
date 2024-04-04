using SOTagsCollector.API.Entities;

namespace SOTagsCollector.API.Clients;

public interface IStackExchangeClient
{
    Task<List<Tag>> GetTagsAsync(GetTagsRequest request);
}