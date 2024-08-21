using MomoAPI.Converter;
using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Segment.DataModel;
public record Reply : BaseMessage
{
    //[JsonConverter(typeof(StringConverter))]
    [JsonPropertyName("id")]
    public long Id { get; set; }
}
