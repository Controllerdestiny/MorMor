using Newtonsoft.Json;

namespace MorMor.Model.Terraria;

public class PlayerOnlineInfo
{
    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("duration")]
    public int Duration { get; init; }
}
