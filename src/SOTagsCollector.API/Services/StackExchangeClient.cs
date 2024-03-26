
namespace SOTagsCollector.API;

public class StackExchangeClient : IStackExchangeClient
{
    public Task<List<Tag>> GetTagsSortedByPopularity(int count)
    {
        throw new NotImplementedException();
    }
}
