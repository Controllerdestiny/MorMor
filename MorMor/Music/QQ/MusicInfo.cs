namespace MorMor.Music.QQ;

public class MusicInfo
{
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string mid { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string vid { get; set; }
    /// <summary>
    /// 花海
    /// </summary>
    public string song { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string cover { get; set; }
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
    /// 
    /// </summary>
    public int type { get; set; }
    /// <summary>
    /// 1个多版本
    /// </summary>
    public string grp { get; set; }
}

public class MusicList
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<MusicInfo> data { get; set; }
}
