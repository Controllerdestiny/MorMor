using Newtonsoft.Json;

namespace MorMor.Model.Terraria;

public class TerrariaPlayerInfo
{
    [JsonProperty("nickname")]
    public string Nick { get; init; }

    [JsonProperty("username")]
    public string Name { get; init; }

    [JsonProperty("group")]
    public string Group { get; init; }

    [JsonProperty("active")]
    public bool Active { get; init; }

    [JsonProperty("state")]
    public int State { get; init; }

    [JsonProperty("team")]
    public int Team { get; init; }
}
