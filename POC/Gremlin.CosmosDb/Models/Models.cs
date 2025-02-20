namespace Gremlin.CosmosDb;

public interface IProperties { }

public class UserVertex(object? id) : Vertex(id, nameof(UserVertex)), IProperties
{
    public object PartitionKey { get; init; } = DateTime.UtcNow.Year;
    public string Name { get; set; } = string.Empty;
    public string? IntroducerId { get; set; }
    public string? ParentId { get; set; }
}

public class OrderVertex(object? id) : Vertex(id, nameof(OrderVertex)), IProperties
{
    public object PartitionKey { get; init; } = DateTime.UtcNow.Year;
    public string UserId { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
}

public class LeftEdge(object id, Vertex outV, Vertex inV) : Edge(id, outV, nameof(LeftEdge), inV) { }

public class RightEdge(object id, Vertex outV, Vertex inV) : Edge(id, outV, nameof(RightEdge), inV) { }

public class IntroducedEdge(object? id, Vertex outV, Vertex inV) : Edge(id, outV, nameof(IntroducedEdge), inV) { }

public class OrderedEdge(object? id, Vertex outV, Vertex inV) : Edge(id, outV, nameof(IntroducedEdge), inV);
public class OrderedEdgeProperty : IProperties
{
    public required DateTime OrderDate { get; set; }
}