namespace MorMor.Music._163;

//如果好用，请收藏地址，帮忙分享。
public class Singer
{
    /// <summary>
    /// 李克勤
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long id { get; set; }
}

public class MusicData
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
    public List<Singer> singers { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string url { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string jumpurl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string pay { get; set; }
}

public class ApiResult
{
    /// <summary>
    /// 
    /// </summary>
    public int code { get; set; }
    /// <summary>
    /// 获取成功
    /// </summary>
    public string message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public MusicData data { get; set; }
}
