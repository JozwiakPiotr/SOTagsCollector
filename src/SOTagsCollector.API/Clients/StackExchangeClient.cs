using System.Text.Json;
using Microsoft.Extensions.Options;
using SOTagsCollector.API.Entities;

namespace SOTagsCollector.API.Clients;

public class StackExchangeClient : IStackExchangeClient
{
    private readonly HttpClient _httpClient;
    private readonly StackExchangeConfig _stackExchangeConfig;
    private readonly JsonSerializerOptions _serializerOptions;

    public StackExchangeClient(
        HttpClient httpClient,
        IOptions<StackExchangeConfig> options)
    {
        _stackExchangeConfig = options.Value;
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    public async Task<List<Tag>> GetTagsAsync(GetTagsRequest request)
    {
        var result = await _httpClient.GetAsync(
            $"tags?site={request.Site}&pagesize={request.PageSize}&order={request.Order}&sort={request.Sort}&page={request.Page}");
        var jsonResult = await result.Content.ReadFromJsonAsync<JsonResult>(_serializerOptions);
        return jsonResult?.Items ?? new List<Tag>();
    }

    public record JsonResult(List<Tag> Items);
}
