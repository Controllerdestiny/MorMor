using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Info;

public readonly struct FriendInfo
{
    [JsonPropertyName("nickname")]
    public string Name { get; init; }

    [JsonPropertyName("user_id")]
    public long UID { get; init; }

    [JsonPropertyName("remark")]
    public string Remark { get; init; }
}
