

using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Segment.DataModel;

public record Json : BaseMessage
{
    [JsonPropertyName("data")]
    public string Connect { get; set; } = string.Empty;
}
