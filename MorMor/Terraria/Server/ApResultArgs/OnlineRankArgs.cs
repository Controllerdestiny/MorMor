using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Terraria.Server.ApResultArgs;

public class OnlineRankArgs :BaseResultArgs
{
    [JsonProperty("response")]
    public string Response { get; init; }

    [JsonProperty("data")]
    public Dictionary<string, int> Rank { get; init; }
}
