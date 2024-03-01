using Newtonsoft.Json;

namespace MomoAPI.Model.Event.NoticeEvent;

internal class OneBotGroupRecallEventArgs : BaseObNoticeEventArgs
{
    [JsonProperty("operator_id")]
    public long OperatorId { get; set; }

    [JsonProperty("message_id")]
    public long MessageId { get; set; }

    [JsonProperty("group_id")]
    public long GroupId { get; set; }
}
