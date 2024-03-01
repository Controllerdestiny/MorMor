using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;

public record Video : BaseMessage
{
    [JsonProperty("file")]
    public string Data { get; set; }
}
