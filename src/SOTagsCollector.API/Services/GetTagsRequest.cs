namespace SOTagsCollector.API.Services;

public record struct GetTagsRequest(int Page, int PageSize, string Order, string Sort, string Site);