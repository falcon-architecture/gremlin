namespace Falcon.CosmosDb.Gremlin.Sample.OpenApi;

public static class HealthCheckExtensions
{
    public static WebApplication UseFalconHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/alive", new HealthCheckOptions
        {
            Predicate = (check) => check.Tags.Contains("liveness")
        });
        app.MapHealthChecks("/ready", new HealthCheckOptions
        {
            Predicate = (check) => check.Tags.Contains("readiness")
        });
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (httpContext, healthReport) =>
            {
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(healthReport));
            }
        });
        return app;
    }
}