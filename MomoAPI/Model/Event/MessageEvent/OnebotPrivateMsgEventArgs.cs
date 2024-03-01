using MomoAPI.Entities.Info;
using Newtonsoft.Json;

namespace MomoAPI.Model.Event.MessageEvent;

/// <summary>
/// 私聊消息事件
/// </summary>
internal sealed class OnebotPrivateMsgEventArgs : BaseObMessageEventArgs
{
    /// <summary>
    /// 发送人信息
    /// </summary>
    [JsonProperty(PropertyName = "sender")]
    internal PrivateSenderInfo SenderInfo { get; set; }
}