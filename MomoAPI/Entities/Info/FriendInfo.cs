using Newtonsoft.Json;

namespace MomoAPI.Entities.Info;

public readonly struct FriendInfo
{
    [JsonProperty("nickname")]
    public string Name { get; }

    [JsonProperty("user_id")]
    public long UID { get; }

    [JsonProperty("remark")]
    public string Remark { get; }
}
