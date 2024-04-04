using System.Net;
using Polly;
using Polly.Contrib.WaitAndRetry;
using SOTagsCollector.API.Clients;

namespace SOTagsCollector.API.Installers;

public static class HttpClientsInstaller
{
    public static WebApplicationBuilder AddHttpClients(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ErrorLoggingHandler>();
        var stackExchangeConfig = new StackExchangeConfig();
        builder.Configuration.GetSection(StackExchangeConfig.SectionName).Bind(stackExchangeConfig);
        
        builder.Services.AddHttpClient<IStackExchangeClient, StackExchangeClient>(client =>
            {
                client.BaseAddress = stackExchangeConfig.BaseUri;
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            })
            .AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)))
            .AddHttpMessageHandler<ErrorLoggingHandler>();
        
        return builder;
    }
}

public class ErrorLoggingHandler : DelegatingHandler
{
    private readonly ILogger<ErrorLoggingHandler> _logger;

    public ErrorLoggingHandler(ILogger<ErrorLoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Request:{RequestMethod} {RequestURL} with body:{RequestBody}, produces response with code:{ResponseStatusCode} and body:{ResponseBody}"
                ,request.Method, request.RequestUri, request.Content, response.StatusCode, response.Content);
        }
        return response;
    }
}