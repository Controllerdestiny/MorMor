using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Model.API;
using MomoAPI.Utils;
using Newtonsoft.Json;


namespace MomoAPI.Model.Event.MessageEvent;

/// <summary>
/// 消息事件基类
/// </summary>
internal abstract class BaseObMessageEventArgs : BaseObApiEventArgs
{
    /// <summary>
    /// 消息类型
    /// </summary>
    [JsonProperty("message_type")]
    internal string MessageType { get; set; }

    /// <summary>
    /// 消息子类型
    /// </summary>
    [JsonProperty("sub_type")]
    [JsonConverter(typeof(EnumConverter))]
    internal SubType SubType { get; set; }

    /// <summary>
    /// 消息 ID
    /// </summary>
    [JsonProperty("message_id")]
    internal long MessageId { get; set; }

    /// <summary>
    /// 发送者UID
    /// </summary>
    [JsonProperty("user_id")]
    internal long UserId { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [JsonProperty("message")]
    internal List<OnebotSegment> MessageList { get; set; }

    /// <summary>
    /// 字体
    /// </summary>
    [JsonProperty("font")]
    internal int Font { get; set; }

    /// <summary>
    /// 原始消息内容
    /// </summary>
    [JsonProperty("raw_message", NullValueHandling = NullValueHandling.Ignore)]
    internal string RawMessage { get; set; }
}