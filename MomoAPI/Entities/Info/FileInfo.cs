using Newtonsoft.Json;

namespace MomoAPI.Entities.Info;

public readonly struct FileInfo
{
    /// <summary>
    /// 文件路径
    /// </summary>
    [JsonProperty("file")]
    public string File { get; init; }

    /// <summary>
    /// 文件名
    /// </summary>
    [JsonProperty("file_name")]
    public string FileName { get; init; }

    /// <summary>
    /// 文件大小
    /// </summary>
    [JsonProperty("file_size")]
    public int FileSize { get; init; }

    /// <summary>
    /// 文件Base64内容
    /// </summary>
    [JsonProperty("base64")]
    public string Base64 { get; init; }
}
