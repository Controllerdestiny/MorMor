using MomoAPI.Utils;
using MorMor.Enumeration;
using MorMor.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MorMor.Terraria.Server.ApiRequestParam;

public class BaseResultArgs
{
    [JsonProperty("status")]
    [JsonConverter(typeof(TerrariaApiStatusConverter))]
    public TerrariaApiStatus Status { get; init; }

    [JsonProperty("error",DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string ErrorMessage { get; init; }

    [JsonIgnore]
    public bool IsSuccess => Status == TerrariaApiStatus.Success;

}
