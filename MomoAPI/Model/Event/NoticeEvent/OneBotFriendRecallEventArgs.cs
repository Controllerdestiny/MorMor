using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.NoticeEvent;

public class OneBotFriendRecallEventArgs : BaseObApiEventArgs
{
    [JsonPropertyName("user_id")]
    public long UID { get; set; }

    [JsonPropertyName("message_id")]
    public long MessageId { get; set; }
}
