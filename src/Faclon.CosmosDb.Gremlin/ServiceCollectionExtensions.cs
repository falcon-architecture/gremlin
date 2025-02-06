namespace Falcon.CosmosDb.Gremlin;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGremlinModule(this IServiceCollection services, IConfiguration configuration, string configPath)
    {
        services.Configure<GremlinDbOptions>(configuration.GetSection(configPath));
        return services.AddGremlinModule();
    }

    public static IServiceCollection AddInfrastructureEntityFramework(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<GremlinDbOptions>(section);
        return services.AddGremlinModule();
    }

    public static IServiceCollection AddInfrastructureEntityFramework(this IServiceCollection services, Action<GremlinDbOptions> configure)
    {
        services.Configure(configure);
        return services.AddGremlinModule();
    }

    private static IServiceCollection AddGremlinModule(this IServiceCollection services)
    {
        services.AddHealthChecks()
                .AddCheck<GremlinHealthChecks>(name: "gremlin", tags: ["gremlin", "readiness"]);

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<GremlinDbOptions>>().Value;
            return g.UseCosmosDb<IVertex, IEdge>(configurator =>
                        configurator.At(new Uri(options.HostName), options.Database, options.GraphName)
                                    .WithPartitionKey(x => x.PartitionKey)
                                    .AuthenticateBy(options.PrimaryKey));
        });
        services.AddScoped<IGremlinClient, GremlinClient>();
        return services;
    }

    public static IApplicationBuilder UseGremlinMiddelwares(IApplicationBuilder app)
    {
        return app;
    }

    public static IApplicationBuilder UseGremlinModule(IApplicationBuilder app)
    {
        return app;
    }
}