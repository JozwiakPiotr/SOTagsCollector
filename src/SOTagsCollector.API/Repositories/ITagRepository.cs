using SOTagsCollector.API.Entities;

namespace SOTagsCollector.API.Repositories;

public interface ITagRepository
{
    Task MergeAsync(List<Tag> tags);
}