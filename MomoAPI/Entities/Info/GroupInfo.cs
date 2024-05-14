using Newtonsoft.Json;

namespace MomoAPI.Entities.Info;

public struct GroupInfo
{
    [JsonProperty("group_name")]
    public string Name { get; init; }

    [JsonProperty("group_id")]
    public long ID { get; init; }

    [JsonProperty("member_count")]
    public int MemberCount { get; init; }

    [JsonProperty("max_member_count")]
    public int MaxMemberCount { get; init; }
}
