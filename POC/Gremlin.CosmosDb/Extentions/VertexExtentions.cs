namespace Gremlin.CosmosDb.Extentions;

public static class VertexExtensions
{
    public static GraphTraversal<Vertex, Vertex> Property(this GraphTraversal<Vertex, Vertex> graph, IProperties properties)
    {
        return graph.Properties(properties.GetProperties());
    }

    public static GraphTraversal<Vertex, Edge> Property(this GraphTraversal<Vertex, Edge> graph, IProperties properties)
    {
        return graph.Properties(properties.GetProperties());
    }

    private static Dictionary<string, object> GetProperties(this IProperties properties)
    {
        var resultProperties = new Dictionary<string, object>();
        var vertexType = properties.GetType();
        var vertexProperties = vertexType.GetProperties();

        foreach (var property in vertexProperties)
        {
            var value = property.GetValue(properties);
            if (value != null)
            {
                resultProperties.Add(property.Name, value);
            }
        }
        return resultProperties;
    }

    static HashSet<string> ExcludeKeys = ["Id", "Label"];
    private static GraphTraversal<Vertex, Vertex> Properties(this GraphTraversal<Vertex, Vertex> graph, Dictionary<string, object> properties)
    {
        foreach (var property in properties)
        {
            var key = property.Key;
            object? value = property.Value switch
            {
                string s => s,
                int i => i,
                long l => l,
                double d => d,
                float f => f,
                bool b => b,
                DateTime dt => dt.ToString("o"),
                _ => property.Value.ToString()
            };
            if (ExcludeKeys.Contains(key) && value is not { }) continue;
            graph.Property(ExcludeKeys.Contains(key) ? key.ToLower() : key, value);
        }
        return graph;
    }

    private static GraphTraversal<Vertex, Edge> Properties(this GraphTraversal<Vertex, Edge> graph, Dictionary<string, object> properties)
    {
        foreach (var property in properties)
        {
            var key = property.Key;
            object? value = property.Value switch
            {
                string s => s,
                int i => i,
                long l => l,
                double d => d,
                float f => f,
                bool b => b,
                DateTime dt => dt.ToString("o"),
                _ => property.Value.ToString()
            };
            if (ExcludeKeys.Contains(key) && value is not { }) continue;
            graph.Property(ExcludeKeys.Contains(key) ? key.ToLower() : key, value);
        }
        return graph;
    }

    public static GraphTraversal<Vertex, IDictionary<string, V>>? Project<V>(this GraphTraversal<Vertex, Vertex> graph) where V : Vertex
    {
        var vertexType = typeof(V);
        var vertexProperties = vertexType.GetProperties().Select(x => x.Name).ToArray();
        var traversal = graph.Project<V>(vertexProperties[0], vertexProperties[1..]);
        foreach (var property in vertexProperties)
        {
            if (property != nameof(Vertex.Id))
            {
                traversal.By(__.Values<string>(property));
            }
            else
            {
                traversal.By(__.Values<object>(property));
            }
        }
        return traversal;
    }
}
