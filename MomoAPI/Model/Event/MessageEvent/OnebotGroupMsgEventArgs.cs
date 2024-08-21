using MomoAPI.Entities.Info;
using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.MessageEvent;

/// <summary>
/// 群组消息事件
/// </summary>
public sealed class OnebotGroupMsgEventArgs : BaseObMessageEventArgs
{
    /// <summary>
    /// 群号
    /// </summary>
    [JsonPropertyName( "group_id")]
    public long GroupId { get; set; }


    /// <summary>
    /// 发送人信息
    /// </summary>
    [JsonPropertyName("sender")]
    public required GroupSenderInfo SenderInfo { get; set; }

}