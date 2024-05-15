using MomoAPI.Enumeration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace MomoAPI.Utils;

public static class Utils
{
    private static HttpClient HttpClient = new();
    public static async Task<string> HttpGet(string url, Dictionary<string, string>? args = null)
    {
        var uriBuilder = new UriBuilder(url);
        var param = HttpUtility.ParseQueryString(uriBuilder.Query);
        if (args != null)
            foreach (var (key, val) in args)
                param[key] = val;
        uriBuilder.Query = param.ToString();
        return await HttpClient.GetStringAsync(uriBuilder.ToString());
    }

    internal static string SignMusic(MusicType type, string jumpUrl, string AudioUrl, string imageUrl, string song, string singer)
    {
        var url = "https://ss.xingzhige.com/music_card/card.php";
        var signtype = type switch
        {
            MusicType.QQ => "qq",
            MusicType._163 => "163",
            _ => "qq"
        };
        var args = new Dictionary<string, string>()
        {
            { "type", signtype },
            { "musicUrl", AudioUrl },
            { "jumpUrl", jumpUrl },
            { "title", song },
            { "get", "yes" },
            { "singer", singer },
            { "preview", imageUrl },
        };
        var result = HttpGet(url, args).Result;
        var data = JObject.Parse(result);
        return JsonConvert.SerializeObject(data?["data"]);
    }


    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (var value in values)
        {
            action.Invoke(value);
        }
    }
}
