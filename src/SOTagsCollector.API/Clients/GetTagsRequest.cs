namespace SOTagsCollector.API.Clients;

public record struct GetTagsRequest(int Page, int PageSize, string Order, string Sort, string Site);