using MomoAPI.Converter;
using MomoAPI.Enumeration.EventParamType;
using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.NoticeEvent;

public class OneBotGroupMemberChangeEventArgs : BaseObNoticeEventArgs
{
    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("operator_id")]
    public long OperatorId { get; set; }

    [JsonPropertyName("sub_type")]
    [JsonConverter(typeof(EnumConverter<MemberChangeType>))]
    public MemberChangeType ChangeType { get; set; }
}
