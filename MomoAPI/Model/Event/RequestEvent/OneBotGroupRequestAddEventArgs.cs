using MomoAPI.Converter;
using MomoAPI.Enumeration.EventParamType;
using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.RequestEvent;

public class OneBotGroupRequestAddEventArgs : BaseObRequestEventArgs
{
    [JsonPropertyName("sub_type")]
    [JsonConverter(typeof(EnumConverter<GroupRequestType>))]
    public GroupRequestType RequestType { get; set; }

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }
}
