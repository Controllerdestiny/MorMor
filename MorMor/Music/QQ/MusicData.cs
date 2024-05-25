namespace MorMor.Music.QQ;


public class MusicData
{
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 花海
    /// </summary>
    public string song { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string subtitle { get; set; }
    /// <summary>
    /// 周杰伦
    /// </summary>
    public string singer { get; set; }
    /// <summary>
    /// 魔杰座
    /// </summary>
    public string album { get; set; }
    /// <summary>
    /// 付费
    /// </summary>
    public string pay { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string time { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string bpm { get; set; }
    /// <summary>
    /// 臻品母带2.0
    /// </summary>
    public string quality { get; set; }
    /// <summary>
    /// 4分24秒
    /// </summary>
    public string interval { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string size { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string kbps { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string cover { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string link { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string url { get; set; }
}

public class Music
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public MusicData data { get; set; }
}
