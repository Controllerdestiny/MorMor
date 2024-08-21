using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event.RequestEvent;

public class BaseObRequestEventArgs : BaseObApiEventArgs
{
    [JsonPropertyName("user_id")]
    public long UID { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; } = string.Empty;

    [JsonPropertyName("flag")]
    public string Flag { get; set; } = string.Empty;
}
