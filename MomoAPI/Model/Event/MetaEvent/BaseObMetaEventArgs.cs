using Newtonsoft.Json;

namespace MomoAPI.Model.Event.MetaEvent;

internal class BaseObMetaEventArgs : BaseObApiEventArgs
{
    [JsonProperty("meta_event_type")]
    public string MeatEventType { get; set; }
}
