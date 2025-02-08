namespace Falcon.CosmosDb.Gremlin.HealthChecks;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddCosmosGremlinHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
               .AddCheck<GremlinHealthChecks>(name: "gremlin", tags: ["azure", "cosmos", "gremlin", "readiness"]);
        return services;
    }
}