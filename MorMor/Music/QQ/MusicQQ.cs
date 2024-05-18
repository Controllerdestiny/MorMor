using Newtonsoft.Json;

namespace MorMor.Music.QQ;

public class MusicQQ
{
    private const string Uri = "https://oiapi.net/API/QQ_Music/";
    public static async Task<List<MusicInfo>> GetMusicList(string name)
    {
        var ret = new List<MusicInfo>();
        var client = new HttpClient();
        var url = $"{Uri}?msg={name}";
        var result = await client.GetStringAsync(url);
        var data = JsonConvert.DeserializeObject<MusicList>(result);
        if (data != null && data.code == 1)
        {
            return data.data;
        }
        return ret;
    }

    public static async Task<MusicData?> GetMusic(string name, int id)
    {
        var client = new HttpClient();
        var url = $"{Uri}?msg={name}&n={id}";
        var result = await client.GetStringAsync(url);
        var data = JsonConvert.DeserializeObject<Music>(result);
        if (data != null && data.code == 1)
        {
            return data.data;
        }
        return null;
    }
}
