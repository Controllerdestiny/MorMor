using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;

public record Record : BaseMessage
{
    /// <summary>
    /// 文件/URL/base64
    /// </summary>
    [JsonProperty("file")]
    public string File { get; init; }

    /// <summary>
    /// 文件ID
    /// </summary>
    [JsonProperty("file_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string FileId { get; init; }

    /// <summary>
    /// 语音URL
    /// </summary>
    [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Url { get; init; }

    /// <summary>
    /// 变声
    /// </summary>
    [JsonProperty("magic", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Magic { get; init; }

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
