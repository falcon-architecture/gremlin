namespace Falcon.CosmosDb.Gremlin.Graph;

public interface IVertex
{
    object Id { get; init; }
    object PartitionKey { get; init; }
    long Level { get; set; }
}

