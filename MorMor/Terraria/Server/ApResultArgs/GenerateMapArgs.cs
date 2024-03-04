using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;

namespace MorMor.Terraria.Server.ApResultArgs;

public class GenerateMapArgs : BaseResultArgs
{
    [JsonProperty("response")]
    public string Uri { get; init; }
}
