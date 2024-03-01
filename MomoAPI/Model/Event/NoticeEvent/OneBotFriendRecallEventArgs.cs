using Newtonsoft.Json;

namespace MomoAPI.Model.Event.NoticeEvent
{
    internal class OneBotFriendRecallEventArgs : BaseObApiEventArgs
    {
        [JsonProperty("user_id")]
        public long UID { get; set; }

        [JsonProperty("message_id")]
        public long MessageId { get; set; }
    }
}
