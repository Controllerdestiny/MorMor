using Newtonsoft.Json;

namespace MorMor.Model.Terraria;

public class Prize
{
    [JsonProperty("奖品名称")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("奖品ID")]
    public int ID { get; set; }

    [JsonProperty("中奖概率")]
    public int Probability { get; set; }

    [JsonProperty("最大数量")]
    public int Max { get; set; }

    [JsonProperty("最小数量")]
    public int Min { get; set; }
}
