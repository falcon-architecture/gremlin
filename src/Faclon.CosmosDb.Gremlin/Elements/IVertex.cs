namespace Falcon.CosmosDb.Gremlin;

public interface IVertex
{
    object Id { get; set; }
    object PartitionKey { get; init; }
}

