using MomoAPI.Converter;
using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Segment.DataModel;

public record At : BaseMessage
{
    [JsonPropertyName("qq")]
    [JsonInclude]
    internal string Target { get; set; } = string.Empty;

    [JsonIgnore]
    public long UserId => long.TryParse(Target, out long Id) ? Id : -1;
}
