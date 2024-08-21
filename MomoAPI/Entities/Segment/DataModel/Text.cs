using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Segment.DataModel;

public record Text : BaseMessage
{
    [JsonPropertyName("text")]
    public string Content { get; init; } = string.Empty;
}
