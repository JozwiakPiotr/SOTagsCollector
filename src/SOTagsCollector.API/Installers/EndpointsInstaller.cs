using System.Reflection;
using SOTagsCollector.API.Common;
using SOTagsCollector.API.Features.Tags.GetTags;
using SOTagsCollector.API.Features.Tags.UpdateTags;

namespace SOTagsCollector.API.Installers;

public static class EndpointsInstaller
{
    public static WebApplicationBuilder AddEndpoints(this WebApplicationBuilder builder)
    {
        builder.Services.Scan(scan =>
            scan.FromAssemblyOf<GetTagsHandler>()
                .AddClasses(c => c.AssignableTo<IEndpoint>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        return builder;
    }

    public static void MapTags(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var group = app.MapGroup("/api/tags")
            .WithTags("tags");
        foreach (var endpoint in scope.ServiceProvider.GetServices<IEndpoint>())
        {
            endpoint.Map(group);
        }
    }
}