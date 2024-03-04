using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Model.Terraria;

public class PlayerOnlineInfo
{
    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("duration")]
    public int Duration { get; init; }
}
