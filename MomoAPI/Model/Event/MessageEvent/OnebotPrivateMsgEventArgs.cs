using MomoAPI.Entities.Info;
using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.MessageEvent;

/// <summary>
/// 私聊消息事件
/// </summary>
public sealed class OnebotPrivateMsgEventArgs : BaseObMessageEventArgs
{
    /// <summary>
    /// 发送人信息
    /// </summary>
    [JsonPropertyName("sender")]
    public required PrivateSenderInfo SenderInfo { get; set; }
}