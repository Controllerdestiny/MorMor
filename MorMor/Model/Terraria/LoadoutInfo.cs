using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Model.Terraria;

public class LoadoutInfo
{
    [JsonProperty("armor")]
    public List<ItemInfo> Armors { get; init; }

    [JsonProperty("dye")]
    public List<ItemInfo> Dyes { get; init; }
}
