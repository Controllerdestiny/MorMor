using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Terraria.Server.ApResultArgs;

public class ExecuteCommamdArgs : BaseResultArgs
{
    [JsonProperty("response")]
    public List<string> Response { get; init; }
}
