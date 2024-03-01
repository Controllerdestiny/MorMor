using Newtonsoft.Json;

namespace MomoAPI.Model.Event.RequestEvent;

internal class BaseObRequestEventArgs : BaseObApiEventArgs
{
    [JsonProperty("user_id")]
    public long UID { get; set; }

    [JsonProperty("comment")]
    public string Comment { get; set; }

    [JsonProperty("flag")]
    public string Flag { get; set; }
}
