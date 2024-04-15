
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Web;

namespace MorMor.Music.Music_QQ;

public class Music_QQ
{
    private HttpClient Client;
    private string SearchUrl => $"https://u.y.qq.com/cgi-bin/musicu.fcg?_webcgikey=DoSearchForQQMusicDesktop&_={GetTimeStamp(false)}";

    private long guid = 1559616839293;

    private Br_Type[] brtype;
    public Music_QQ()
    {
        Client = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip
        });
        Client.DefaultRequestHeaders.Add("Connection", "keep-alive");
        Client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en-US;q=0.8,en;q=0.7");
        Client.DefaultRequestHeaders.Add("Accept", "application/json");
        Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Linux; Android 13; 22127RK46C Build/TKQ1.220905.001) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.97 Mobile Safari/537.36");
        Client.DefaultRequestHeaders.Add("Origin", "https://i.y.qq.com");
        Client.DefaultRequestHeaders.Add("Host", "u.y.qq.com");
        Client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        Client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");
        Client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
        Client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
        Client.DefaultRequestHeaders.Add("Referer", "https://i.y.qq.com/");
        Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
        brtype = new Br_Type[8]
        {
            new Br_Type("size_try",960887,"AI00","flac"),
            new Br_Type("size_flac",999,"F000","flac"),
            new Br_Type("size_320mp3",320,"M800","mp3"),
            new Br_Type("size_192aac",192,"C600","m4a"),
            new Br_Type("size_128mp3",128,"M500","mp3"),
            new Br_Type("size_96aac",96,"M400","m4a"),
            new Br_Type("size_48aac",48,"M200","m4a"),
            new Br_Type("size_24aac",24,"M100","m4a"),
        };
    }

    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <param name="bflag">秒或毫秒</param>
    /// <returns></returns>
    public string GetTimeStamp(bool bflag)
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        string ret;
        if (bflag)
            ret = Convert.ToInt64(ts.TotalSeconds).ToString();
        else
            ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

        return ret;
    }

    public async Task<QQmusicAlbum> GetMusicData(string mid, QQCookie Cookie)
    {
        var param = new
        {
            comm = new
            {
                format = "json",
                inCharset = "utf-8",
                outCharset = "utf-8",
                notice = 0,
                platform = "h5",
                uin = Cookie.Uin,
                authst = Cookie.Musickey
            },
            req_0 = new
            {
                module = "music.pf_song_detail_svr",
                method = "get_song_detail",
                param = new
                {
                    song_mid = mid,
                    song_type = 0
                }
            }
        };
        var Content = await Client.PostAsync($"https://u.y.qq.com/cgi-bin/musicu.fcg", JsonContent.Create(param));
        var data = (JObject)JsonConvert.DeserializeObject(await Content.Content.ReadAsStringAsync())!;
        if (data.Value<int>("code") != 0)
            throw new Exception("API接口_0出错，获取歌曲失败!");
        var album_mid = data.Value<JObject>("req_0")!.Value<JObject>("data")!.Value<JObject>("track_info")!.Value<JObject>("album")!.Value<string>("mid")!;
        var media_mid = data.Value<JObject>("req_0")!.Value<JObject>("data")!.Value<JObject>("track_info")!.Value<JObject>("file")!.Value<string>("media_mid")!;
        var type = data.Value<JObject>("req_0")!.Value<JObject>("data")!.Value<JObject>("track_info")!.Value<int>("type")!;
        var name = data.Value<JObject>("req_0")!.Value<JObject>("data")!.Value<JObject>("track_info")!.Value<string>("name")!;
        var singers = data.Value<JObject>("req_0")!.Value<JObject>("data")!.Value<JObject>("track_info")!.Value<JArray>("singer")!;
        var _singers = new List<string>();
        for (int n = 0; n < singers.Count; n++)
        {
            _singers.Add(singers[n].Value<string>("name")!);
        }
        return new QQmusicAlbum(album_mid, media_mid, type,name, _singers);
    }

    public async Task<QQMusicData> GetMusicByMid(string mid, QQCookie Cookie, int br = 4)
    {
        var albumData = await GetMusicData(mid, Cookie);
        var param = new
        {
            comm = new
            {
                format = "json",
                platform = "yqq.json",
                needNewCode = 1,
                ct = 24,
                cv =  5381,
                uin = Cookie.Uin,
                authst = Cookie.Musickey
            },
            req_0 = new
            {
                module = "vkey.GetVkeyServer",
                method = "CgiGetVkey",
                param = new
                {
                    guid = Guid.NewGuid().ToString("N"),
                    songmid = new string[8],
                    filename = new string[8],
                    songtype = new int[8],
                    uin = "0",
                    loginflag = 0,
                    platform = "yqq.json"
                }
            }
        };
        for (int i = 0; i < brtype.Length; i++)
        {
            param.req_0.param.songmid[i] = mid;
            param.req_0.param.filename[i] = $"{brtype[i].type}{albumData.Media_mid}.{brtype[i].suffix}";
            param.req_0.param.songtype[i] = albumData.Type;
        }
        var Content = await Client.PostAsync($"https://u.y.qq.com/cgi-bin/musicu.fcg", JsonContent.Create(param));
        var data = (JObject)JsonConvert.DeserializeObject(await Content.Content.ReadAsStringAsync())!;

        var vkeys = data.Value<JObject>("req_0")!.Value<JObject>("data")!.Value<JArray>("midurlinfo")!;
        for (int i = br; i < brtype.Length; i++)
        {
            if (vkeys[br] is not null)
            {
                if (string.IsNullOrEmpty(vkeys[br].Value<string>("purl")))
                    throw new Exception("不正确的cookie或cookie已失效!");
                var sip = data.Value<JObject>("req_0")!.Value<JObject>("data")!.Value<JArray>("sip")!;
                var musicUrl = $"{(sip.Count > 0 ? sip[0] : "http://ws.stream.qqmusic.qq.com/")}{vkeys[br].Value<string>("purl")}";
                var pic = $"https://y.gtimg.cn/music/photo_new/T002R800x800M000{albumData.Mid}.jpg?max_age=2592000";
                var jump = $"http://y.qq.com/n/yqq/song/{mid}.html";
                return new QQMusicData()
                {
                    Name = albumData.Name,
                    MusicUrl = musicUrl,
                    JumpUrl = jump,
                    Picture = pic,
                    Singers = albumData.Singers.ToList(),
                    Mid = mid
                };
            }
        }
        throw new Exception("不正确的cookie或cookie已失效!");
    }

    public async Task<QQMusicData> GetMusic(ChoiceMusicSetting setting)
    {
        var musicList = await GetMusicListByName(setting.Name, setting.Cookie);
        setting.Index = setting.Index > 0 ? setting.Index - 1 : 0;
            
        if (musicList.Count > setting.Index)
        {
            var musicInfo = musicList[setting.Index];
            var mid = musicInfo.Mid;
            return await GetMusicByMid(mid, setting.Cookie, setting.br);
        }
        else
        {
            throw new Exception("无法读取到API数据!");
        }
    }

    public async Task<List<QQMusicData>> GetMusicListByName(string name, QQCookie Cookie)
    {
        var body = new
        {
            comm = new comm(Cookie.Uin, Cookie.Musickey),
            req_0 = new req_0(name)
        };
        //设置请求体
        StringContent content = new(JsonConvert.SerializeObject(body, Formatting.Indented), Encoding.UTF8, "application/x-www-form-urlencoded");
        //post请求
        var response = await Client.PostAsync(SearchUrl, content);
        //获取返回
        var result = await response.Content.ReadAsStringAsync();
        var musicInfo = (JObject)JsonConvert.DeserializeObject(result)!;
        if (musicInfo.Value<int>("code") != 0)
        {
            throw new Exception("API返回错误，无法获取音乐信息!");
        }
       var songs = musicInfo.Value<JObject>("req_0")!.Value<JObject>("data")!.Value<JObject>("body")!.Value<JObject>("song")!.Value<JArray>("list")!;
        var res = new List<QQMusicData>();
        foreach (var song in songs)
        {
            var singers = new List<string>();
            var singerData = song.Value<JArray>("singer")!;
            for (int i = 0; i < singerData.Count; i++)
            {
                singers.Add(singerData[i].Value<string>("name")!);
            }
            var mid = song.Value<string>("mid")!;
            var musicName = song.Value<string>("name")!;
            var jump = $"http://y.qq.com/n/yqq/song/{mid}.html";
            res.Add(new QQMusicData()
            {
                Mid = mid,
                Name = musicName,
                JumpUrl = jump,
                Singers = singers
            });
        }
        return res;
    }
}
