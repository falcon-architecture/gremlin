namespace Falcon.CosmosDb.Gremlin.HealthChecks;

public class GremlinHealthChecks : IHealthCheck
{
    private readonly IGremlinQuerySource _gremlinQuerySource;
    public GremlinHealthChecks(IGremlinQuerySource gremlinQuerySource)
    {
        _gremlinQuerySource = gremlinQuerySource;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _gremlinQuerySource.V()
                                    .Limit(0)
                                    .ToArrayAsync(cancellationToken)
                                    .ConfigureAwait(false);
            return HealthCheckResult.Healthy("Gremlin server connection is healthy.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(exception.Message, exception);
        }
    }
}