using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Info;

public class PrivateSenderInfo
{
    /// <summary>
    /// 昵称
    /// </summary>
    [JsonPropertyName("nickname")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("user_id")]
    public long QQ { get; init; }

    [JsonPropertyName("card")]
    public string Card { get; init; } = string.Empty;
}
