namespace MorMor.Music.QQ;

public class MusicInfo
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
}

public class MusicList
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
    public List<MusicInfo> data { get; set; }
}
