using Newtonsoft.Json;

namespace MorMor.Music.QQ;

public class MusicItem
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("mid")]
    public string Mid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("vid")]
    public string Vid { get; set; }

    /// <summary>
    /// 花海
    /// </summary>
    [JsonProperty("song")]
    public string Song { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("cover")]
    public string Cover { get; set; }

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
    /// 
    /// </summary>
    [JsonProperty("type")]
    public int Type { get; set; }

    /// <summary>
    /// 0个多版本
    /// </summary>
    [JsonProperty("grp")]
    public string Grp { get; set; }
}