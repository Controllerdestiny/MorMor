using System.Text.Json;
using System.Text.Json.Serialization;   

namespace MomoAPI.Converter;

internal class StringConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetInt64();
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}