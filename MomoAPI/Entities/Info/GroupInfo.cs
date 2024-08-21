using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Info;

public struct GroupInfo
{
    [JsonPropertyName("group_name")]
    public string Name { get; init; }

    [JsonPropertyName("group_id")]
    public long ID { get; init; }

    [JsonPropertyName("member_count")]
    public int MemberCount { get; init; }

    [JsonPropertyName("max_member_count")]
    public int MaxMemberCount { get; init; }
}
