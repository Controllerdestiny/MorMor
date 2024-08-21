

using MomoAPI.Converter;
using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Info;

public readonly struct FileInfo
{
    /// <summary>
    /// 文件路径
    /// </summary>
    [JsonPropertyName("file")]
    public string File { get; init; }

    /// <summary>
    /// 文件名
    /// </summary>
    [JsonPropertyName("file_name")]
    public string FileName { get; init; }

    /// <summary>
    /// 文件大小
    /// </summary>
    [JsonPropertyName("file_size")]
    public int FileSize { get; init; }

    /// <summary>
    /// 文件Base64内容
    /// </summary>
    [JsonPropertyName("base64")]
    public string Base64 { get; init; }
}
