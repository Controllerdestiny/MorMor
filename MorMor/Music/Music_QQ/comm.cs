namespace MorMor.Music.Music_QQ;
public class comm
{
    public long g_tk = 1617175180;

    public long uin = 2830877581;

    public string format = "json";

    public string inCharset = "utf-8";

    public string outCharset = "utf-8";

    public int notice = 0;

    public string platform = "h5";

    public int needNewCode = 1;

    public int ct = 23;

    public int cv = 0;

    public string authst = "";

    public comm(long uin, string authst)
    {
        this.uin = uin;
        this.authst = authst;
    }
}

public class req_0
{
    public string method = "DoSearchForQQMusicDesktop";

    public string module = "music.search.SearchCgiService";

    private string Name = string.Empty;

    public req_0(string name)
    {
        Name = name;
    }

    public object param => new
    {
        remoteplace = "txt.mqq.all",
        searchid = 1559616839299,
        search_type = 0,
        query = Name,
        page_num = 1,
        num_per_page = 20
    };
}
