
using MorMor.Model.Terraria;
using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;

namespace MorMor.Terraria.Server.ApResultArgs;

public class PlayerListArgs : BaseResultArgs
{
    [JsonProperty("players")]
    public List<TerrariaPlayerInfo> PlayerInfos { get; init; }

    [JsonIgnore]
    public List<string> Players => PlayerInfos.Select(x => x.Name).ToList();
}
