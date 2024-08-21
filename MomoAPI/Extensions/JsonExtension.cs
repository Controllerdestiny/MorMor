using MomoAPI.Resolver;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MomoAPI.Extensions;

public static class JsonExtension
{
    private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        TypeInfoResolver = new PrivateConstructorContractResolver(),
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
    };

    public static T? ToObject<T>(this JsonNode node)
    { 
        return JsonSerializer.Deserialize<T>(node, JsonSerializerOptions);
    }

    public static T? ToObject<T>(this JsonObject obj)
    {
        return JsonSerializer.Deserialize<T>(obj, JsonSerializerOptions);
    }

    public static T? ToObject<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }

    public static string ToJson<T>(this T obj)
    { 
        return JsonSerializer.Serialize(obj, JsonSerializerOptions);
    }
}
