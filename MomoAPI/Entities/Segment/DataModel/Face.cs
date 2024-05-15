using MomoAPI.Converter;
using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;

public record Face : BaseMessage
{
    [JsonConverter(typeof(StringConverter))]
    [JsonProperty("id")]
    public int Id { get; internal set; }
}
