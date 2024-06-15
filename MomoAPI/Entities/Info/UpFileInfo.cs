using Newtonsoft.Json;

namespace MomoAPI.Entities.Info;

public struct UpFileInfo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("fileModelId")]
    public string FileModelId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("fileId")]
    public string FileId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("fileName")]
    public string FileName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("fileSize")]
    public string FileSize { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("busId")]
    public int BusId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("uploadedSize")]
    public string UploadedSize { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("uploadTime")]
    public int UploadTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("deadTime")]
    public int DeadTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("modifyTime")]
    public int ModifyTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("downloadTimes")]
    public int DownloadTimes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("sha")]
    public string Sha { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("sha3")]
    public string Sha3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("md5")]
    public string Md5 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("uploaderLocalPath")]
    public string UploaderLocalPath { get; set; }

    /// <summary>
    /// 沫沫
    /// </summary>
    [JsonProperty("uploaderName")]
    public string UploaderName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("uploaderUin")]
    public string UploaderUin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("parentFolderId")]
    public string ParentFolderId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("localPath")]
    public string LocalPath { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("transStatus")]
    public int TransStatus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("transType")]
    public int TransType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("elementId")]
    public string ElementId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("isFolder")]
    public bool IsFolder { get; set; }
}
