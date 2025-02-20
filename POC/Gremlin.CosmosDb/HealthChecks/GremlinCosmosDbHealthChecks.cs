namespace Gremlin.CosmosDb.HealthChecks;

public class GremlinCosmosDbHealthChecks : IHealthCheck
{
    private readonly GraphTraversalSource _g;
    public GremlinCosmosDbHealthChecks(GraphTraversalSource g)
    {
        _g = g;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _g.V().Count().Promise(t => t, cancellationToken);
            return HealthCheckResult.Healthy("Gremlin db server is healthy");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(exception.Message, exception);
        }
    }
}