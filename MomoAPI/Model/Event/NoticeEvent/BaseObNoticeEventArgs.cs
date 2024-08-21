using MomoAPI.Converter;
using MomoAPI.Enumeration.EventParamType;
using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.NoticeEvent;

/// <summary>
/// 通知事件
/// </summary>
public class BaseObNoticeEventArgs : BaseObApiEventArgs
{
    [JsonPropertyName("notice_type")]
    [JsonConverter(typeof(EnumConverter<NoticeType>))]
    public NoticeType NoticeType { get; set; }

    [JsonPropertyName("user_id")]
    public long Uid { get; set; }
}
