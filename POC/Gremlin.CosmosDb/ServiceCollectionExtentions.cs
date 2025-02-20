namespace Gremlin.CosmosDb;

public static class ServiceCollectionExtensions
{
    private interface IModuleMarker { }
    private sealed class ModuleMarker : IModuleMarker { }

    public static IServiceCollection AddGremlinCosmosDbModule(this IServiceCollection services, IConfiguration configuration, string configPath)
    {
        services.Configure<GremlinOptions>(configuration.GetSection(configPath));
        return services.AddGremlinCosmosDbModule();
    }

    public static IServiceCollection AddGremlinCosmosDbModule(this IServiceCollection services, IConfigurationSection section)
    {
        services.Configure<GremlinOptions>(section);
        return services.AddGremlinCosmosDbModule();
    }

    public static IServiceCollection AddGremlinCosmosDbModule(this IServiceCollection services, Action<GremlinOptions> configure)
    {
        services.Configure(configure);
        return services.AddGremlinCosmosDbModule();
    }

    public static IServiceCollection AddGremlinCosmosDbModule(this IServiceCollection services)
    {
        if (services.Any(s => s.ServiceType == typeof(IModuleMarker)))
        {
            return services;
        }
        services.AddHealthChecks().AddCheck<GremlinCosmosDbHealthChecks>(name: "GremlinCosmosDb", tags: ["GremlinCosmosDb", "readiness"]);
        services.AddSingleton<IModuleMarker, ModuleMarker>()
                .AddSingleton(p => p.GetRequiredService<IOptions<GremlinOptions>>().Value.GetGremlinClient())
                .AddSingleton(p => AnonymousTraversalSource.Traversal().WithRemote(new DriverRemoteConnection(p.GetRequiredService<GremlinClient>())))
                .AddScoped<GremlinService>();
        return services;
    }

    public static IServiceProvider UseGremlinCosmosDbMiddelwares(this IServiceProvider serviceProvider)
    {
        return serviceProvider;
    }

    public static IServiceProvider UseGremlinCosmosDbModule(this IServiceProvider serviceProvider)
    {
        return serviceProvider;
    }
}