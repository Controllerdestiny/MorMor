namespace MorMor.Music.QQ;


public class MusicData
{
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
    public string mid { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int songid { get; set; }
    /// <summary>
    /// 这一生关于你的风景
    /// </summary>
    public string song { get; set; }
    /// <summary>
    /// 枯木逢春
    /// </summary>
    public string singer { get; set; }
    /// <summary>
    /// 这一生关于你的风景
    /// </summary>
    public string album { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> singerList { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string picture { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string pay { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int interval { get; set; }
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
