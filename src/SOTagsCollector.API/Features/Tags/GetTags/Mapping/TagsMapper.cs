using SOTagsCollector.API.Entities;
using SOTagsCollector.API.Persistence;

namespace SOTagsCollector.API.Features.Tags.GetTags.Mapping;

public static class TagsMapper
{
    public static List<GetTagsResponse.Tag> ToResponse(this List<Tag> tags, int population)
    {
        return tags.Select(t => new GetTagsResponse.Tag(t.Name, t.Count, CalculateRatio(t.Count, population))).ToList();
    }
    
    private static double CalculateRatio(int individualCount, int population)
    {
        return Math.Round((double)individualCount / population * 100.0, 2);
    }
}