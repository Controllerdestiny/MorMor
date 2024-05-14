using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;

public record Music : BaseMessage
{
    [JsonProperty("type")]
    public string Type { get; set; } = "custom";

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("audio")]
    public string Audio { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("singer")]
    public string Singer { get; set; }

}
