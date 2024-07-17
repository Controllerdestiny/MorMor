using Newtonsoft.Json;

namespace MorMor.Music.QQ;


public class MusicData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// 花海
    /// </summary>
    [JsonProperty("song")]
    public string Song { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("subtitle")]
    public string Subtitle { get; set; }

    /// <summary>
    /// 周杰伦
    /// </summary>
    [JsonProperty("singer")]
    public string Singer { get; set; }

    /// <summary>
    /// 魔杰座
    /// </summary>
    [JsonProperty("album")]
    public string Album { get; set; }

    /// <summary>
    /// 付费
    /// </summary>
    [JsonProperty("pay")]
    public string Pay { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("bpm")]
    public string Bpm { get; set; }

    /// <summary>
    /// SQ无损音质
    /// </summary>
    [JsonProperty("quality")]
    public string Quality { get; set; }

    /// <summary>
    /// 4分24秒
    /// </summary>
    [JsonProperty("interval")]
    public string Interval { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("size")]
    public string Size { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("kbps")]
    public string Kbps { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("cover")]
    public string Cover { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("link")]
    public string Link { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; }
}