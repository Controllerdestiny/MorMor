using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Utils;
using Newtonsoft.Json;

namespace MomoAPI.Model.Event.NoticeEvent;

/// <summary>
/// 通知事件
/// </summary>
internal class BaseObNoticeEventArgs : BaseObApiEventArgs
{
    [JsonProperty("notice_type")]
    [JsonConverter(typeof(EnumConverter))]
    public NoticeType NoticeType { get; set; }

    [JsonProperty("user_id")]
    public long Uid { get; set; }
}
