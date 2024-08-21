using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.NoticeEvent;

public class OneBotGroupRecallEventArgs : BaseObNoticeEventArgs
{
    [JsonPropertyName("operator_id")]
    public long OperatorId { get; set; }

    [JsonPropertyName("message_id")]
    public long MessageId { get; set; }

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }
}
