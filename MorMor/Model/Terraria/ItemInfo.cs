using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Model.Terraria;

public class ItemInfo
{
    [JsonProperty("netID")]
    public int NetID { get; set; }

    [JsonProperty("stack")]
    public int Stack { get; set; }

    [JsonProperty("prefix")]
    public int Prefix { get; set; }
}
