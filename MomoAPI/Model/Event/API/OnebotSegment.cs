using MomoAPI.Enumeration;
using MomoAPI.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MomoAPI.Model.API;

/// <summary>
/// Onebot消息段
/// </summary>
internal struct OnebotSegment
{
    /// <summary>
    /// 消息段类型
    /// </summary>
    [JsonConverter(typeof(EnumConverter))]
    [JsonProperty("type")]
    internal SegmentType MsgType { get; set; }

    /// <summary>
    /// 消息段JSON
    /// </summary>
    [JsonProperty("data")]
    internal JToken RawData { get; set; }
}