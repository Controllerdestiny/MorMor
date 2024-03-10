using MomoAPI.Utils;
using MorMor.Enumeration;
using Newtonsoft.Json;

namespace MorMor.Model.Terraria.SocketMessageModel;

public class TerrariaMessageContext
{
    [JsonProperty("type")]
    [JsonConverter(typeof(EnumConverter))]
    public SocketMessageType Type { get; set; }

    [JsonProperty("text")]
    public string Message { get; set; }

    [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("color")]
    public byte[] Color { get; set; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}
