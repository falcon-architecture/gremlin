namespace Falcon.CosmosDb.Gremlin.Sample.OpenApi;

public static class OpenApiExtensions
{
    public static IServiceCollection AddFalconOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi();
        // services.AddSwaggerGen(c =>
        // {
        //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Falcon.CosmosDb.Gremlin.Sample", Version = "v1" });
        // });
        return services;
    }

    public static WebApplication UseFalconOpenApi(this WebApplication app, string title)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", title));
        }
        return app;
    }
}