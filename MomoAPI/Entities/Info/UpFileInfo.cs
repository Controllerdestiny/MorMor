using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Info;

public struct UpFileInfo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("fileModelId")]
    public string FileModelId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("fileId")]
    public string FileId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("fileName")]
    public string FileName { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("fileSize")]
    public string FileSize { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("busId")]
    public int BusId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("uploadedSize")]
    public string UploadedSize { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("uploadTime")]
    public int UploadTime { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("deadTime")]
    public int DeadTime { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("modifyTime")]
    public int ModifyTime { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("downloadTimes")]
    public int DownloadTimes { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("sha")]
    public string Sha { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("sha3")]
    public string Sha3 { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("md5")]
    public string Md5 { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("uploaderLocalPath")]
    public string UploaderLocalPath { get; init; }

    /// <summary>
    /// 沫沫
    /// </summary>
    [JsonPropertyName("uploaderName")]
    public string UploaderName { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("uploaderUin")]
    public string UploaderUin { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("parentFolderId")]
    public string ParentFolderId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("localPath")]
    public string LocalPath { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("transStatus")]
    public int TransStatus { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("transType")]
    public int TransType { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("elementId")]
    public string ElementId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("isFolder")]
    public bool IsFolder { get; init; }
}
