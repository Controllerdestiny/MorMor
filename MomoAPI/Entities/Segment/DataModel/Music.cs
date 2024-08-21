

using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Segment.DataModel;

public record Music : BaseMessage
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "custom";

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("audio")]
    public string Audio { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;

    [JsonPropertyName("singer")]
    public string Singer { get; set; } = string.Empty;

}
