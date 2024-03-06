
using Newtonsoft.Json;

namespace MorMor.Model.Terraria;

public class LoadoutInfo
{
    [JsonProperty("armor")]
    public List<ItemInfo> Armors { get; init; }

    [JsonProperty("dye")]
    public List<ItemInfo> Dyes { get; init; }
}
