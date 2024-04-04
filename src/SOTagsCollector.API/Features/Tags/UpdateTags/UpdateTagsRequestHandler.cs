using SOTagsCollector.API.Common;
using SOTagsCollector.API.Installers;
using SOTagsCollector.API.Services;

namespace SOTagsCollector.API.Features.Tags.UpdateTags;

public class UpdateTagsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("update", (
            [AsParameters] UpdateTagsRequest request,
            IRequestHandler<UpdateTagsRequest> h) => h.HandleAsync(request))
            .Produces(200);
    }
}

public class UpdateTagsRequestHandler : IRequestHandler<UpdateTagsRequest>
{
    private readonly ITagService _tagService;
    
    public UpdateTagsRequestHandler(ITagService tagService)
    {
        _tagService = tagService;
    }
    
    public async Task<IResult> HandleAsync(UpdateTagsRequest _)
    {
        await _tagService.UpdateAll();
        return Results.Ok();
    }
}

public record UpdateTagsRequest() : IRequest {}
