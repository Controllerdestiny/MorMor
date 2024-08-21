using MomoAPI.Converter;
using MomoAPI.Entities;
using MomoAPI.Enumeration.EventParamType;
using System.Text.Json.Serialization;

namespace MomoAPI.Model.API;

/// <summary>
/// 发送消息调用参数
/// </summary>
public struct SendMessageParams
{
    /// <summary>
    /// 消息类型 群/私聊
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MessageType>))]
    [JsonPropertyName("message_type")]
    public MessageType MessageType { get; set; }

    /// <summary>
    /// 用户id
    /// </summary>
    [JsonPropertyName("user_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? UserId { get; set; }

    /// <summary>
    /// 群号
    /// </summary>
    [JsonPropertyName("group_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long? GroupId { get; set; }

    /// <summary>
    /// 消息段数组
    /// </summary>
    [JsonPropertyName("message")]
    public MessageBody Message { get; set; }

    /// <summary>
    /// 是否忽略消息段
    /// </summary>
    [JsonPropertyName("auto_escape")]
    public bool AutoEscape { get; set; }
}