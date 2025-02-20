namespace Gremlin.CosmosDb.Configurations;

public class GremlinGraphSON2Reader : GraphSON2Reader
{
    public override dynamic? ToObject(JsonElement graphSon)
    {
        return graphSon.ValueKind switch
        {
            JsonValueKind.Number when graphSon.TryGetInt64(out var longValue) => longValue,
            JsonValueKind.Number when graphSon.TryGetDecimal(out var decimalValue) => decimalValue,
            _ => base.ToObject(graphSon)
        };
    }
}