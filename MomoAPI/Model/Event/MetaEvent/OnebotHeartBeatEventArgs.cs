using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.MetaEvent;

public class OnebotHeartBeatEventArgs : BaseObMetaEventArgs
{
    [JsonPropertyName("status")]
    public JsonObject Status { get; set; } = [];

    [JsonPropertyName("interval")]
    public int Interval { get; set; }
}
