using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MorMor.Music.QQ;

public class ApiRespone
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("data")]
    public JToken Data { get; set; }
}
