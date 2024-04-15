using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;

public record Video : BaseMessage
{
    [JsonProperty("file")]
    public string Data { get; set; }

    [JsonProperty("name",DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Name { get; set; }
}
