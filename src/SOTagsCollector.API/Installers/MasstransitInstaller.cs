using System.Reflection;
using MassTransit;
using SOTagsCollector.API.Handlers;

namespace SOTagsCollector.API.Installers;

public static class MasstransitInstaller
{
    public static WebApplicationBuilder AddMessaging(this WebApplicationBuilder builder)
    {
        var cs = builder.Configuration.GetConnectionString("rabbitmq")!;
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(UpdateTagsHandler).Assembly);
            x.UsingRabbitMq((ctx, cfg) =>
            {
                // cfg.UseMessageRetry(r =>
                //     r.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(16), TimeSpan.FromSeconds(2)));
                cfg.ConfigureEndpoints(ctx);
                cfg.Host(new Uri(cs));
            });
        });
        return builder;
    }
}