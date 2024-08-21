using MomoAPI.Converter;
using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Segment.DataModel;

public record Face : BaseMessage
{
    [JsonConverter(typeof(StringConverter))]
    [JsonPropertyName("id")]
    public int Id { get; internal set; }
}
