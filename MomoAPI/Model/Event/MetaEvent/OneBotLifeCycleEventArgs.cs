using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.MetaEvent;

public class OneBotLifeCycleEventArgs : BaseObMetaEventArgs
{
    [JsonPropertyName("sub_type")]
    public string SubType { get; set; } = string.Empty;
}
