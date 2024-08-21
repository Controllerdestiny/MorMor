using MomoAPI.Converter;
using MomoAPI.Enumeration;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MomoAPI.Model.API;

/// <summary>
/// Onebot消息段
/// </summary>
public struct OnebotSegment
{
    /// <summary>
    /// 消息段类型
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SegmentType>))]
    [JsonPropertyName("type")]
    public SegmentType MsgType { get; init; }

    /// <summary>
    /// 消息段JSON
    /// </summary>
    [JsonPropertyName("data")]
    public JsonObject RawData { get; init; }
}