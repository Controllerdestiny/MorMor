using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MomoAPI.Model.Event.MetaEvent;

internal class OnebotHeartBeatEventArgs : BaseObMetaEventArgs
{
    [JsonProperty("status")]
    public JObject Status { get; set; }

    [JsonProperty("interval")]
    public int Interval { get; set; }
}
