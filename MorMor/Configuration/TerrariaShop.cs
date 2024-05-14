using MorMor.Model.Terraria;
using Newtonsoft.Json;

namespace MorMor.Configuration;

public class TerrariaShop
{
    [JsonProperty("泰拉商店")]
    public List<Shop> TrShop { get; set; } = [];

    public Shop? GetShop(string Name)
    {
        return TrShop.Find(x => x.Name == Name);
    }

    public Shop? GetShop(int id)
    {
        if (id > 0 && id <= TrShop.Count)
            return TrShop[id - 1];
        return null;
    }

    public bool TryGetShop(int id, out Shop? shop)
    {
        shop = GetShop(id);
        return shop != null;
    }

    public bool TryGetShop(string name, out Shop? shop)
    {
        shop = GetShop(name);
        return shop != null;
    }
}
