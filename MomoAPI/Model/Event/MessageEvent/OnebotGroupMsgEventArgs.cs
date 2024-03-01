using MomoAPI.Entities.Info;
using Newtonsoft.Json;

namespace MomoAPI.Model.Event.MessageEvent;

/// <summary>
/// 群组消息事件
/// </summary>
internal sealed class OnebotGroupMsgEventArgs : BaseObMessageEventArgs
{
    /// <summary>
    /// 群号
    /// </summary>
    [JsonProperty(PropertyName = "group_id")]
    internal long GroupId { get; set; }


    /// <summary>
    /// 发送人信息
    /// </summary>
    [JsonProperty(PropertyName = "sender")]
    internal GroupSenderInfo SenderInfo { get; set; }

}