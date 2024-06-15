using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomoAPI.Entities.Info;

public struct PeerInfo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("peerId")]
    public string PeerId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("type")]
    public int Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("folderInfo")]
    public FolderInfo? FolderInfo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("fileInfo")]
    public UpFileInfo? FileInfo { get; set; }
}
