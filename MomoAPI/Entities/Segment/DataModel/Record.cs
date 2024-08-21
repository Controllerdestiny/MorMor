using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Segment.DataModel;

public record Record : BaseMessage
{
    /// <summary>
    /// 文件/URL/base64
    /// </summary>
    [JsonPropertyName("file")]
    public string File { get; init; } = string.Empty;

    /// <summary>
    /// 文件ID
    /// </summary>
    [JsonPropertyName("file_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string FileId { get; init; } = string.Empty;

    /// <summary>
    /// 语音URL
    /// </summary>
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Url { get; init; } = string.Empty;

    /// <summary>
    /// 变声
    /// </summary>
    [JsonPropertyName("magic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Magic { get; init; } = string.Empty;

    /// <summary>
    /// 缓存
    /// </summary>
    [JsonPropertyName("cache")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Cache { get; internal set; }

    /// <summary>
    /// 代理
    /// </summary>
    [JsonPropertyName("proxy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Proxy { get; internal set; }
}
