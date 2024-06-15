using Newtonsoft.Json;

namespace MorMor.Music.QQ;

public class MusicQQ
{
    private const string Uri = "https://oiapi.net/API/QQ_Music/";
    public static async Task<List<MusicInfo>> GetMusicList(string name)
    {
        var ret = new List<MusicInfo>();
        var param = new Dictionary<string, string>()
        {
            { "msg", name },
        };
        var res = await MomoAPI.Utils.Utils.HttpGet(Uri, param);
        var data = JsonConvert.DeserializeObject<MusicList>(res);
        if (data != null && data.code == 1)
        {
            return data.data;
        }
        return ret;
    }

    public static async Task<MusicData?> GetMusic(string name, int id)
    {
        var param = new Dictionary<string, string>()
        {
            { "msg", name },
            { "n", id.ToString()}
        };
        var res = await MomoAPI.Utils.Utils.HttpGet(Uri, param);
        var data = JsonConvert.DeserializeObject<Music>(res);
        if (data != null && data.code == 1)
        {
            return data.data;
        }
        return null;
    }
}
