using System.Linq.Expressions;
using SOTagsCollector.API.Entities;
using SOTagsCollector.API.Persistance;

namespace SOTagsCollector.API.Features.V1.Tags;

public class GetTagsEndpoint
{
    private readonly TagsDb _tagsDb;

    public GetTagsEndpoint(TagsDb tagsDb)
    {
        _tagsDb = tagsDb;
    }

    public async Task<IResult> HandleAsync(GetTagsRequest request)
    {
        throw new NotImplementedException();
    }

    private static Dictionary<string, Expression<Func<Tag, object>>> _sortExpressions = new()
    {
        [nameof(Tag.Name)] = t => t.Name,
        [nameof(Tag.Count)] = t => t.Count
    };
}

public record GetTagsRequest(int Page, int PageSize, string? Order, string? Sort);

public record GetTagsResponse(int Page, int PageSize, List<GetTagsResponse.Tag> Tags, int MaxCount)
{
    public record Tag(string Name, int Count, double PopulationRatio);
}
