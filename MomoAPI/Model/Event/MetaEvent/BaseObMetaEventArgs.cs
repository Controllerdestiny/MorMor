using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.MetaEvent;

public class BaseObMetaEventArgs : BaseObApiEventArgs
{
    [JsonPropertyName("meta_event_type")]
    public string MeatEventType { get; set; } = string.Empty;
}
