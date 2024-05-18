
using Newtonsoft.Json;

namespace MorMor.Music._163;

public class Music163
{
    public const string Uri = "https://oiapi.net/API/Music_163/";

    public static async Task<List<MusicInfo>> GetMusicList(string name)
    {
        var ret = new List<MusicInfo>();
        var client = new HttpClient();
        var url = $"{Uri}?name={name}";
        var result = await client.GetStringAsync(url);
        var data = JsonConvert.DeserializeObject<Response>(result);
        if (data != null && data.code == 0)
        {
            return data.data;
        }
        return ret;
    }

    public static async Task<MusicData?> GetMusic(string name, int id)
    {
        var client = new HttpClient();
        var url = $"{Uri}?name={name}&n={id}";
        var result = await client.GetStringAsync(url);
        var data = JsonConvert.DeserializeObject<ApiResult>(result);
        if (data != null && data.code == 0)
        {
            return data.data;
        }
        return null;
    }
}
