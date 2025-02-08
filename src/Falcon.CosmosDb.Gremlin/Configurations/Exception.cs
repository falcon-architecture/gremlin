namespace Falcon.CosmosDb.Gremlin.Configurations;

[Serializable]
public class GremlinException : Exception
{
    public GremlinException(string message) : base(message) { }
    public GremlinException(string message, Exception innerException) : base(message, innerException) { }
}