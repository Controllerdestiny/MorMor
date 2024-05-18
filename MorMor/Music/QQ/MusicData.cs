namespace MorMor.Music.QQ;


public class MusicData
{
    /// <summary>
    /// 
    /// </summary>
    public int songid { get; set; }
    /// <summary>
    /// 夜曲
    /// </summary>
    public string song { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> singers { get; set; }
    /// <summary>
    /// 十一月的萧邦
    /// </summary>
    public string album { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string mid { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string picture { get; set; }
    /// <summary>
    /// 周杰伦
    /// </summary>
    public string singer { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string music { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string size { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int time { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int br { get; set; }
}

public class Music
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>

    /// </summary>
    public string message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public MusicData data { get; set; }
}