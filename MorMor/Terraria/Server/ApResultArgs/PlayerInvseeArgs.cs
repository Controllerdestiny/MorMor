using MorMor.Model.Terraria;
using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;

namespace MorMor.Terraria.Server.ApResultArgs;

public class PlayerInvseeArgs : BaseResultArgs
{
    [JsonProperty("response")]
    public string Response { get; init; }

    [JsonProperty("data")]
    public PlayerinventoryInfo PlayerinventoryInfo { get; init; }
}
