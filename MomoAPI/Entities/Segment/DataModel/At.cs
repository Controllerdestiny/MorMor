using MomoAPI.Converter;
using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;

public record At : BaseMessage
{
    [JsonConverter(typeof(StringConverter))]
    [JsonProperty("qq")]
    internal string Target { get; set; }

    [JsonIgnore]
    public long UserId => long.TryParse(Target, out long Id) ? Id : -1;
}
