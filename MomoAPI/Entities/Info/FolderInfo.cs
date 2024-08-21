using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Info;

public struct FolderInfo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("folderId")]
    public string FolderId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("parentFolderId")]
    public string ParentFolderId { get; init; }

    /// <summary>
    /// 解析
    /// </summary>
    [JsonPropertyName("folderName")]
    public string FolderName { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("createTime")]
    public int CreateTime { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("modifyTime")]
    public int ModifyTime { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("createUin")]
    public string CreateUin { get; init; }

    /// <summary>
    /// 无终lsp
    /// </summary>
    [JsonPropertyName("creatorName")]
    public string CreatorName { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("totalFileCount")]
    public int TotalFileCount { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("modifyUin")]
    public string ModifyUin { get; init; }

    /// <summary>
    /// 无终lsp
    /// </summary>
    [JsonPropertyName("modifyName")]
    public string ModifyName { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("usedSpace")]
    public string UsedSpace { get; init; }
}
