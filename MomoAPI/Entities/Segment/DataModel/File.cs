using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;

public record File : BaseMessage
{
    [JsonProperty("file")]
    public string Data { get; set; }
}
