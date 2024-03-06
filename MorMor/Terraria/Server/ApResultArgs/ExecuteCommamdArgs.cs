using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;

namespace MorMor.Terraria.Server.ApResultArgs;

public class ExecuteCommamdArgs : BaseResultArgs
{
    [JsonProperty("response")]
    public List<string> Response { get; init; }
}
