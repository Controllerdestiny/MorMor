using Newtonsoft.Json;

namespace MomoAPI.Entities.Info;

public readonly struct GroupInfo
{
    [JsonProperty("group_name")]
    public string Name { get; }

    [JsonProperty("group_id")]
    public int ID { get; }

    [JsonProperty("member_count")]
    public int MemberCount { get; }

    [JsonProperty("max_member_count")]
    public int MaxMemberCount { get; }
}
