using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomoAPI.Entities.Info;

public struct FolderInfo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("folderId")]
    public string FolderId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("parentFolderId")]
    public string ParentFolderId { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    [JsonProperty("folderName")]
    public string FolderName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("createTime")]
    public int CreateTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("modifyTime")]
    public int ModifyTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("createUin")]
    public string CreateUin { get; set; }

    /// <summary>
    /// 无终lsp
    /// </summary>
    [JsonProperty("creatorName")]
    public string CreatorName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("totalFileCount")]
    public int TotalFileCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("modifyUin")]
    public string ModifyUin { get; set; }

    /// <summary>
    /// 无终lsp
    /// </summary>
    [JsonProperty("modifyName")]
    public string ModifyName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("usedSpace")]
    public string UsedSpace { get; set; }
}
