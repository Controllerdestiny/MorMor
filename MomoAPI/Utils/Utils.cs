using MomoAPI.Enumeration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace MomoAPI.Utils;

public static class Utils
{
    private readonly static HttpClient HttpClient = new();
    public static async Task<string> HttpGet(string url, Dictionary<string, string>? args = null)
    {
        var uriBuilder = new UriBuilder(url);
        var param = HttpUtility.ParseQueryString(uriBuilder.Query);
        if (args != null)
            foreach (var (key, val) in args)
                param[key] = val;
        uriBuilder.Query = param.ToString();
        //await Console.Out.WriteLineAsync(uriBuilder.ToString());
        return await HttpClient.GetStringAsync(uriBuilder.ToString());
    }

    public static async Task<string> HttpPost(string url, Dictionary<string, string>? args = null)
    {
        FormUrlEncodedContent form = new(args);
        var content = await HttpClient.PostAsync(url, form);
        return await content.Content.ReadAsStringAsync();
    }

    internal static string SignMusic(MusicType type, string jumpUrl, string AudioUrl, string imageUrl, string song, string singer)
    {
        var url = "http://oiapi.net/API/QQMusicJSONArk";
        var signtype = type switch
        {
            MusicType.QQ => "qq",
            MusicType._163 => "163",
            _ => "qq"
        };
        //var (status, res) = MomoAPI.Net.OneBotAPI.Instance.GetCookie("qzone.qq.com").Result;
        var args = new Dictionary<string, string>()
        {
            { "format", signtype },
            { "url", AudioUrl },
            { "jump", jumpUrl },
            { "song", song.Replace(","," ")},
            { "singer", singer },
            { "cover", imageUrl },
            //{ "p_skey", res.Pskey },
            //{ "uin", MomoAPI.Net.OneBotAPI.Instance.BotId.ToString() },
        };
        var result = HttpPost(url, args).Result;
        var data = JObject.Parse(result);
        //data["data"]["meta"]["music"]["uin"] = 424993442;
        return JsonConvert.SerializeObject(data?["data"]);
    }

    //internal static string SignMusic(MusicType type, string jumpUrl, string AudioUrl, string imageUrl, string song, string singer)
    //{
    //    var url = "";
    //    var signtype = type switch
    //    {
    //        MusicType.QQ => "qq",
    //        MusicType._163 => "163",
    //        _ => "163"
    //    };
    //    //var (status, res) = OneBotAPI.Instance.GetCookie("qzone.qq.com").Result;
    //    var args = new Dictionary<string, string>()
    //    {
    //        { "type", signtype },
    //        { "musicUrl", AudioUrl },
    //        { "jumpUrl", jumpUrl },
    //        { "title", song },
    //        { "singer", singer },
    //        { "preview", imageUrl },
    //        { "get", "yes" },
    //    };
    //    var result = HttpGet(url, args).Result;
    //    var data = JObject.Parse(result);
    //    return JsonConvert.SerializeObject(data?["data"]);
    //}


    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (var value in values)
        {
            action.Invoke(value);
        }
    }
}
