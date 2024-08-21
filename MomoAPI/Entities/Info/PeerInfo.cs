using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Info;

public struct PeerInfo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("peerId")]
    public string PeerId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("type")]
    public int Type { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("folderInfo")]
    public FolderInfo? FolderInfo { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("fileInfo")]
    public UpFileInfo? FileInfo { get; set; }
}
