using Newtonsoft.Json;

namespace MomoAPI.Entities.Info;

public class PrivateSenderInfo
{
    /// <summary>
    /// 昵称
    /// </summary>
    [JsonProperty("nickname")]
    public string Name { get; internal set; }

    [JsonProperty("user_id")]
    public long QQ { get; internal set; }

    [JsonProperty("card")]
    public string Card { get; internal set; }
}
