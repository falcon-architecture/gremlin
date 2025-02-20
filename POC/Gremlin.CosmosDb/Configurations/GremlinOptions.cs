namespace Gremlin.CosmosDb.Configurations;

public class GremlinOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public int Port { get; set; }
    public string AuthKey { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string Collection { get; set; } = string.Empty;
    public string PartitionKey { get; set; } = string.Empty;

    public GremlinServer GetGremlinServer()
    {
        return new GremlinServer(
            Endpoint, 
            Port,
            enableSsl: true,
            username: $"/dbs/{Database}/colls/{Collection}",
            password: AuthKey
        );
    }
    public GremlinClient GetGremlinClient()
    {
        return new GremlinClient(GetGremlinServer(), new GraphSON2MessageSerializer(new GremlinGraphSON2Reader()));
    }
}
