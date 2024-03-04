using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;

namespace MorMor.Terraria.Server.ApResultArgs;

public class DeatRankArgs: BaseResultArgs
{
    [JsonProperty("response")]
    public string Response { get; init; }

    [JsonProperty("data")]
    public Dictionary<string, int> Rank { get; init; }
}
