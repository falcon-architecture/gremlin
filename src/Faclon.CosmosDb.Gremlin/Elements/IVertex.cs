namespace Falcon.CosmosDb.Gremlin;

public interface IVertex
{
    string? Id { get; set; }
    object PartitionKey { get; init; }
    long Level { get; set; }
}

