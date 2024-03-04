using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;

namespace MorMor.Terraria.Server.ApResultArgs;

public class EconomicsBankArgs : BaseResultArgs
{
    [JsonProperty("Currency")]
    public long CurrentNum { get; init; }

    [JsonProperty("response")]
    public string Response { get; init; }
}
