using Newtonsoft.Json;

namespace Test;

internal class CommentConvert : JsonConverter
{
    public readonly string Value;
    public CommentConvert(string value) 
    { 
        this.Value = value;
    }
    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        return reader.Value;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
        writer.WriteComment(Value);
    }
}
