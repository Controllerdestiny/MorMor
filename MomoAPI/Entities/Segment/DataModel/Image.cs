using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;

public record Image : BaseMessage
{
    /// <summary>
    /// 文件/URL/base64
    /// </summary>
    [JsonProperty("file")]
    public string File { get; init; }

    /// <summary>
    /// 图片URL
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; init; }

    /// <summary>
    /// ID
    /// </summary>
    [JsonProperty("file_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string FileId { get; init; }
    /// <summary>
    /// 类型
    /// </summary>
    [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Type { get; init; }

    /// <summary>
    /// 缓存
    /// </summary>
    [JsonProperty("cache", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Cache { get; internal set; }

    /// <summary>
    /// 代理
    /// </summary>
    [JsonProperty("proxy", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Proxy { get; internal set; }
}
