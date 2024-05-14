using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Utils;
using Newtonsoft.Json;


namespace MomoAPI.Model.Event.NoticeEvent;

internal class OneBotGroupMuteEventArgs : BaseObNoticeEventArgs
{
    [JsonProperty("sub_type")]
    [JsonConverter(typeof(EnumConverter))]
    public MuteType OperatorType { get; set; }

    [JsonProperty("group_id")]
    public long GroupId { get; set; }

    [JsonProperty("operator_id")]
    public long OperatorId { get; set; }

    [JsonProperty("user_id")]
    public long TargetId { get; set; }

    [JsonProperty("duration")]
    public int Duration { get; set; }
}
