using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;

public record Text : BaseMessage
{
    [JsonProperty("text")]
    public string Content { get; init; }
}
