namespace SOTagsCollector.API;

public interface IStackExchangeClient
{
    Task<List<Tag>> GetTagsSortedByPopularity(int count);
}
