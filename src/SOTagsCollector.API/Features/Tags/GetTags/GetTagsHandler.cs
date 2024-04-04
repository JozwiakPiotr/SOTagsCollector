using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SOTagsCollector.API.Common;
using SOTagsCollector.API.Entities;
using SOTagsCollector.API.Features.Tags.GetTags.Mapping;
using SOTagsCollector.API.Persistence;
using Swashbuckle.AspNetCore.Annotations;

namespace SOTagsCollector.API.Features.Tags.GetTags;

public class GetTagsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("", (
                [AsParameters] GetTagsRequest request,
                IRequestHandler<GetTagsRequest> h) => h.HandleAsync(request))
            .Produces<GetTagsResponse>()
            .Produces(404);
    }
}
public class GetTagsHandler : IRequestHandler<GetTagsRequest>
{
    private readonly TagsDb _tagsDb;

    public GetTagsHandler(TagsDb tagsDb)
    {
        _tagsDb = tagsDb;
    }
    public async Task<IResult> HandleAsync(GetTagsRequest request)
    {
        SortExpressions.TryGetValue(request.Sort, out var sortExpression);
        if (sortExpression is null)
            return Results.NotFound();

        IOrderedQueryable<Tag> orderedTags;
        
        if (request.Order == "desc")
            orderedTags = _tagsDb.Tags.OrderByDescending(sortExpression);
        else if (request.Order == "asc")
            orderedTags = _tagsDb.Tags.OrderBy(sortExpression);
        else
            return Results.NotFound();

        var tags = await orderedTags
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var population = await _tagsDb.Tags.SumAsync(t => t.Count);
        var totalCount = await _tagsDb.Tags.CountAsync();
        var result = new GetTagsResponse(
            request.Page,
            request.PageSize,
            tags.ToResponse(population),
            totalCount);

        return Results.Ok(result);
    }
    
    private static readonly Dictionary<string, Expression<Func<Tag, object>>> SortExpressions = new()
    {
        [nameof(Tag.Name).ToLowerInvariant()] = t => t.Name,
        [nameof(Tag.Count).ToLowerInvariant()] = t => t.Count
    };
}

public record GetTagsRequest(int Page, int PageSize, string Order, string Sort) : IRequest;

public record GetTagsResponse(int Page, int PageSize, List<GetTagsResponse.Tag> Tags, int TotalCount)
{
    public record Tag(string Name, int Count, double PopulationRatio);
}