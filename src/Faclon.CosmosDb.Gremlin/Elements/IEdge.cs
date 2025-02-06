namespace Falcon.CosmosDb.Gremlin;

public interface IEdge
{
    string? Id { get; init; }
}
public record Left : IEdge
{
    public string? Id { get; init; }
}
public record Right : IEdge
{
    public string? Id { get; init; }
}
