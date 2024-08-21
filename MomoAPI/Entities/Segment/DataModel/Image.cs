
using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Segment.DataModel;

public record Image : BaseMessage
{
    /// <summary>
    /// 文件/URL/base64
    /// </summary>
    [JsonPropertyName("file")]
    public string File { get; set; } = string.Empty;

    /// <summary>
    /// 图片URL
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;

    /// <summary>
    /// 外显
    /// </summary>
    [JsonPropertyName("summary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Summary { get; init; } = string.Empty;

    /// <summary>
    /// ID
    /// </summary>
    [JsonPropertyName("file_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string FileId { get; init; } = string.Empty;
    /// <summary>
    /// 类型
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Type { get; init; } = string.Empty;

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
