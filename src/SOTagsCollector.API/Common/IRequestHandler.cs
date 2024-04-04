namespace SOTagsCollector.API.Common;

public interface IRequestHandler<in TRequest> 
    where TRequest : IRequest
{
    Task<IResult> HandleAsync(TRequest request);
}