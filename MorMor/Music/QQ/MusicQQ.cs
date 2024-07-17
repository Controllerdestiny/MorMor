using Newtonsoft.Json;

namespace MorMor.Music.QQ;

public class MusicQQ
{
    private const string Uri = "https://api.lolimi.cn/API/yiny/";
    public static async Task<List<MusicItem>> GetMusicList(string name)
    {
        var ret = new List<MusicItem>();
        var param = new Dictionary<string, string>()
        {
            { "word", name },
        };
        var res = await MomoAPI.Utils.Utils.HttpGet(Uri, param);
        var data = JsonConvert.DeserializeObject<ApiRespone>(res);
        if (data != null && data.Code == 200)
        {
            return data.Data.ToObject<List<MusicItem>>()!;
        }
        return ret;
    }

    public static async Task<MusicData?> GetMusic(string name, int id)
    {
        var param = new Dictionary<string, string>()
        {
            { "word", name },
            { "n", id.ToString()}
        };
        var res = await MomoAPI.Utils.Utils.HttpGet(Uri, param);
        var data = JsonConvert.DeserializeObject<ApiRespone>(res);
        if (data != null && data.Code == 200)
        {
            return data.Data.ToObject<MusicData>();
        }
        return null;
    }
}
