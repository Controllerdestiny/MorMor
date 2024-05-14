using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;

public record Json : BaseMessage
{
    [JsonProperty("data")]
    public string Connect { get; set; }
}
