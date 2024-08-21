

using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Segment.DataModel;

public record File : BaseMessage
{
    [JsonPropertyName("file")]
    public string Data { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("file_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string FileId { get; set; } = string.Empty;
}
