using Newtonsoft.Json;

namespace MomoAPI.Model.Event.MetaEvent;

internal class OneBotLifeCycleEventArgs : BaseObMetaEventArgs
{
    [JsonProperty("sub_type")]
    public string SubType { get; set; }
}
