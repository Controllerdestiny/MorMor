
using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Info;

public readonly struct GroupUpLoadInfo
{
    [JsonPropertyName("id")]
    public string ID { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("size")]
    public int Size { get; init; }

    [JsonPropertyName("busid")]
    public int Busid { get; init; }
}
