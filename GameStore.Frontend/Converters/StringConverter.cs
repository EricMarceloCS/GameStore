using System.Text.Json.Serialization;
using System.Text.Json;

namespace GameStore.Frontend.Converters;

public class StringConverter : JsonConverter<string?>
{
    public override string? Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32().ToString();
        }

        return reader.GetString();
    }

    public override void Write(System.Text.Json.Utf8JsonWriter writer, string? value, System.Text.Json.JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}

