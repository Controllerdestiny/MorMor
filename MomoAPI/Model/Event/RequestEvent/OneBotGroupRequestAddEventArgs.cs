using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Utils;
using Newtonsoft.Json;

namespace MomoAPI.Model.Event.RequestEvent;

internal class OneBotGroupRequestAddEventArgs : BaseObRequestEventArgs
{
    [JsonProperty("sub_type")]
    [JsonConverter(typeof(EnumConverter))]
    public GroupRequestType RequestType { get; set; }

    [JsonProperty("group_id")]
    public long GroupId { get; set; }
}
