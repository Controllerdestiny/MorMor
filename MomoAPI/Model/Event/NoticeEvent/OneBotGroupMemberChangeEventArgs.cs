using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Utils;
using Newtonsoft.Json;

namespace MomoAPI.Model.Event.NoticeEvent;

internal class OneBotGroupMemberChangeEventArgs : BaseObNoticeEventArgs
{
    [JsonProperty("group_id")]
    public long GroupId { get; set; }

    [JsonProperty("operator_id")]
    public long OperatorId { get; set; }

    [JsonProperty("sub_type")]
    [JsonConverter(typeof(EnumConverter))]
    public MemberChangeType ChangeType { get; set; }
}
