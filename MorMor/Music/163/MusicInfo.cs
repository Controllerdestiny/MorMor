namespace MorMor.Music._163;

public class MusicInfo
{
    /// <summary>
    /// 月半小夜曲
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string picurl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string jumpurl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<Singer> singers { get; set; }
}

public class Response
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 搜索成功
    /// </summary>
    public string message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<MusicInfo> data { get; set; }
}
