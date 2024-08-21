using MomoAPI.Converter;
using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Model.API;
using System.Text.Json.Serialization;


namespace MomoAPI.Model.Event.MessageEvent;

/// <summary>
/// 消息事件基类
/// </summary>
public abstract class BaseObMessageEventArgs : BaseObApiEventArgs
{
    /// <summary>
    /// 消息类型
    /// </summary>
    [JsonPropertyName("message_type")]
    public string MessageType { get; set; } = string.Empty;

    /// <summary>
    /// 消息子类型
    /// </summary>
    [JsonPropertyName("sub_type")]
    [JsonConverter(typeof(EnumConverter<SubType>))]
    public SubType SubType { get; set; }

    /// <summary>
    /// 消息 ID
    /// </summary>
    [JsonPropertyName("message_id")]
    public long MessageId { get; set; }

    /// <summary>
    /// 发送者UID
    /// </summary>
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [JsonPropertyName("message")]
    public List<OnebotSegment> MessageList { get; set; } = [];

    /// <summary>
    /// 字体
    /// </summary>
    [JsonPropertyName("font")]
    public int Font { get; set; }

    /// <summary>
    /// 原始消息内容
    /// </summary>
    [JsonPropertyName("raw_message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string RawMessage { get; set; } = string.Empty;
}