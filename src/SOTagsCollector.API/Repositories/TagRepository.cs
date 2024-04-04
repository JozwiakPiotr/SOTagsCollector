using Microsoft.EntityFrameworkCore;
using SOTagsCollector.API.Entities;
using SOTagsCollector.API.Persistence;

namespace SOTagsCollector.API.Repositories;

public class TagRepository : ITagRepository
{
    private readonly TagsDb _tagsDb;

    public TagRepository(TagsDb tagsDb)
    {
        _tagsDb = tagsDb;
    }
    public async Task MergeAsync(List<Tag> tags)
    {
        foreach (var tag in tags)
        {
            if (await _tagsDb.Tags.AnyAsync(t => t.Name == tag.Name))
            {
                _tagsDb.Tags.Update(tag);
            }
            else
            {
                _tagsDb.Tags.Add(tag);
            }
        }
        await _tagsDb.SaveChangesAsync();
    }
}