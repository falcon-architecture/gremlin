namespace Falcon.CosmosDb.Gremlin;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGremlinModule(this IServiceCollection services, IConfiguration configuration, string configPath)
    {
        services.Configure<GremlinDbOptions>(configuration.GetSection(configPath));
        return services.AddGremlinModule();
    }

    public static IServiceCollection AddGremlinModule(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<GremlinDbOptions>(section);
        return services.AddGremlinModule();
    }

    public static IServiceCollection AddGremlinModule(this IServiceCollection services, Action<GremlinDbOptions> configure)
    {
        services.Configure(configure);
        return services.AddGremlinModule();
    }

    private static IServiceCollection AddGremlinModule(this IServiceCollection services)
    {
        services.AddCosmosGremlinHealthChecks()
                .AddGremlinq(setup => setup
                    .UseCosmosDb<IVertex, IEdge>()
                    .UseNewtonsoftJson());

        // .AddGremlinq(setup => setup
        //     .UseCosmosDb<IVertex, IEdge>()
        //      .Configure((configurator, config) => {
        //         return configurator
        //         .At(new Uri(builder.Configuration["Gremlinqa:CosmosDb:Uri"]))
        //         .OnDatabase(builder.Configuration["Gremlinqa:CosmosDb:Database"])
        //         .OnGraph(builder.Configuration["Gremlinqa:CosmosDb:Graph"])
        //         .WithPartitionKey(x => x.PartitionKey)
        //         .AuthenticateBy(builder.Configuration["Gremlinqa:CosmosDb:AuthKey"]);
        //     )
        //     .UseNewtonsoftJson());
        // .AddSingleton(provider =>
        // {
        //     var options = provider.GetRequiredService<IOptions<GremlinDbOptions>>().Value;
        //     return g.UseCosmosDb<IVertex, IEdge>(configurator =>
        //                 configurator.At(new Uri(options.HostName), options.Database, options.GraphName)
        //                             .WithPartitionKey(x => x.PartitionKey)
        //                             .AuthenticateBy(options.PrimaryKey)
        //                             .UseNewtonsoftJson());
        // });
        // .AddScoped<IGremlinClient, GremlinClient>();
        return services;
    }

    public static IServiceProvider UseGremlinMiddelwares(this IServiceProvider serviceProvider)
    {
        return serviceProvider;
    }

    public static IServiceProvider UseGremlinModule(this IServiceProvider serviceProvider)
    {
        return serviceProvider;
    }
}