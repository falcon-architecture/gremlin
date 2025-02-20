namespace Gremlin.CosmosDb.Extentions;

public static class TraversalExtensions
{
    public static string ToQuery(this Bytecode bytecode)
    {
        return GroovyTranslator.Of("g").Translate(bytecode);
    }

    public static string ToQuery(this ITraversal traversal)
    {
        return traversal.Bytecode.ToQuery();
    }

    public static Task ExecuteAsync(this ITraversal traversal, GremlinClient client, CancellationToken cancellationToken = default)
    {
        string query = traversal.ToQuery();
        Console.WriteLine($"Query: {query}");
        return client.SubmitAsync(query, cancellationToken: cancellationToken);
    }
    public static Task<ResultSet<T>> ExecuteAsync<T>(this ITraversal traversal, GremlinClient client, CancellationToken cancellationToken = default)
    {
        string query = traversal.ToQuery();
        Console.WriteLine($"Query: {query}");
        return client.SubmitAsync<T>(query, cancellationToken: cancellationToken);
    }
    public static Task<ResultSet<Dictionary<string, object>>> ExecuteDynamicAsync(this ITraversal traversal, GremlinClient client, CancellationToken cancellationToken = default)
    {
        string query = traversal.ToQuery();
        Console.WriteLine($"Query: {query}");
        return client.SubmitAsync<Dictionary<string, object>>(query, cancellationToken: cancellationToken);
    }
    public static Task<T?> ExecuteSingleAsync<T>(this ITraversal traversal, GremlinClient client, CancellationToken cancellationToken = default)
    {
        string query = traversal.ToQuery();
        Console.WriteLine($"Query: {query}");
        return client.SubmitWithSingleResultAsync<T>(query, cancellationToken: cancellationToken);
    }
}
