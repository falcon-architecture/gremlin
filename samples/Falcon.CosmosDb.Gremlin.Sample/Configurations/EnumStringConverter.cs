namespace Falcon.CosmosDb.Gremlin.Sample.OpenApi;

public class EnumStringConverter : JsonConverter<Enum>
{
    public override Enum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? enumString = reader.GetString();
        ArgumentNullException.ThrowIfNull(enumString);
        if (!typeToConvert.IsEnum) { throw new JsonException("Type must be an enum."); }
        try
        {
            return (Enum)Enum.Parse(typeToConvert, enumString, ignoreCase: true);
        }
        catch (ArgumentException exception)
        {
            throw new JsonException($"Unable to convert \"{enumString}\" to enum \"{typeToConvert}\".", exception);
        }
    }

    public override void Write(Utf8JsonWriter writer, Enum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}