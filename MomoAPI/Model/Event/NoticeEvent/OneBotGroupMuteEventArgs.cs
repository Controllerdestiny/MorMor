using MomoAPI.Converter;
using MomoAPI.Enumeration.EventParamType;
using System.Text.Json.Serialization;


namespace MomoAPI.Model.Event.NoticeEvent;

public class OneBotGroupMuteEventArgs : BaseObNoticeEventArgs
{
    [JsonPropertyName("sub_type")]
    [JsonConverter(typeof(EnumConverter<MuteType>))]
    public MuteType OperatorType { get; set; }

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("operator_id")]
    public long OperatorId { get; set; }

    [JsonPropertyName("user_id")]
    public long TargetId { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }
}
