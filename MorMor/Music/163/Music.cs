using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace MomoAPI.Music._163;
public class Music_163
{
    private const string encSecKey = "5cab2b2073b315b420815a56c81f57f62b396655bd04d5005fb1de166811c631551b1da456fdc6eb45f87ae926b19c74a06b3fc6a04c5018a52d5960a44cae7eeafd543a2b1735f4a60f74c23ef487ed15c1f40f35af40fdb66ec4fc67fe991fdefd3266ea8f55cddbaabb0f75f8b19c1c7eca6447dec6938969045e04851929";

    private const string iv = "ABxeF5MspBB0AbUJ";

    private const string ig = "0CoJUm6Qyw8W8jud";

    private const string key = "0102030405060708";

    private const string musicList = "https://music.163.com/weapi/cloudsearch/get/web?csrf_token=";

    private const string playMusic = "https://music.163.com/weapi/song/enhance/player/url/v1?csrf_token=";

    private const string musicinfo = "https://music.163.com/weapi/v3/song/detail?csrf_token=";

    private readonly HttpClient Client;

    public Music_163()
    {
        Client = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip
        });
        Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.79");
        Client.DefaultRequestHeaders.Add("Origin", "https://music.163.com");
        Client.DefaultRequestHeaders.Add("Referer", "https://music.163.com/");
        //Client.DefaultRequestHeaders.Add("Cookie", "NMTID=00ODL8lt-ZqSkZ5e06dla4fOqToajwAAAGKqlWYOw; _iuqxldmzr_=32; _ntes_nnid=714f2c58b8a04c9c5decd8e39883930a,1695074851964; _ntes_nuid=714f2c58b8a04c9c5decd8e39883930a; WEVNSM=1.0.0; WNMCID=gwqrnx.1695074855886.01.0; ntes_utid=tid._.%252BZHv5Klg7IxFQlQUVFPQliX2imQk%252BvkM._.0; sDeviceId=YD-67yCTgsi55BEE1UAQUbRPSfpeyKsFVgQ; WM_TID=h1VlksryEfdBVVQQEQaQh2Cn2zEzVZPE; __snaker__id=xQLBh2BlsmaYEYl7; gdxidpyhxdE=8xvXE37L%5CCRIAzl%5CUBdNdpO3zonMOM6sCpHEbX3Tl0BuQo2L9B2Wi%2BvxDHTpBtoT%5CXP43Z%5CAqP%2FTZ1qG7BtWCc4V%2FsX7M7ZmwQetHo%2Fj4cK3Vrxa1A7NE%2FVp5S6zEj8AKOi6yCj2i4mXxn%2FwrDujRryAy0BMYpUq%2FOkimXnbJro%2F7fZG%3A1697578736382; YD00000558929251%3AWM_NI=scl5iNtYAAk3tmm%2BxISqCPjkmhBmVNK7jpZtq2GSFPrONOya1oArYucZ2aNzBa8hUbYdZLqgRPsBUgXss8cOAHl0KhWQWyObfkgj0BytI7COnuun%2BE94LJcjKt7rqhk2Mjg%3D; YD00000558929251%3AWM_NIKE=9ca17ae2e6ffcda170e2e6eed8f265f6bff792c87b8bb88aa3c85b828a9eb1d467a9eafa89d2748a8b9c99e92af0fea7c3b92ab1bda4d6ec46b8878fa2ea39b8b3a3ccfb4391af8db1e961afb88e8ed54689e9a691d980b3ee89b4c55cb29182b0fc6bf3b18f93dc7fa186fe82cd7282bff889d86a9bbc96adf46d9290bda3d86d9cadbdd9e2458398a4d1ca64958f999ace6aa6b5a98bb34abbac87d8c245aeabbb8faa4ffbec84abe26289acf993f464a9ee838be637e2a3; YD00000558929251%3AWM_TID=1MavOVX0tJZEQQVRVQaUmx%2BvgGhYRTwn; MUSIC_U=00F6820DB81B74F58E755B5CDEF2668DC29E3468043085CD560D8FEB1F536E7DC501C4EADBF5367C8FD3D2204BAD489E26A37083E46F7EF3499C203D6B86B51445E3740F4634DFF5F5A0F446F62BDA7088AC058A5B094719B989C0517EB0D05A8A89D4ADE859C46119F1F286664BB42715D082642056F150A37FD77439B074573CC34BDBC80EA7D9343BEFE3CBFCC541B9F0C2B0585559975D7B9BDCD6C64370AD2923C250E2276475E967F81FAA7F81B0A416C8706E6D6A2DF3057AA0E14F831CE3606EBD9B5F4B484CE1E8FE04EBFF03D3E4F9264EE1373E46F05E8B155C864F36845B308BF81AAB3A3CCF246B1F5CC0B0ED841EA5D7C912E1614F4832A49C98F1E865E77F07FF40D31405F2F30F301F4805099A5768E148CF1E81FBD6C6FB9F51FBE1301D5F79848DFB3E100592976D0B4EC1B167548DA5AD7CEE1A6ECBD9C9E2539D0DC5B871C1CD7E20B92E5C3D162634C73CC177B45C7B2F02BA15743A56119A450BCE5F1104A7E8851F67575553; __remember_me=true; ntes_kaola_ad=1; __csrf=c7ec74d0510752522e0327ae7a4533c4; __csrf=c7ec74d0510752522e0327ae7a4533c4; WM_NI=2t5gJVh6cBfpVdvivHbqkyqit2dODFc0pZMiM%2BQJr3bC%2F0vCQhhW1OFAO74Ia0ss7jwdNMCmlt1XUmu4uYvbiJfLECJsJt7mB5f6cSqYafP%2FE9aaZL7ewkR3ssbhpLY8SEY%3D; WM_NIKE=9ca17ae2e6ffcda170e2e6eea5d73fa9adb695d26e869a8aa7d55a879e8b83d439fcb2b8a3d561948bfe9bc72af0fea7c3b92af2acfb88c95eaceaada3e57c96bcba8af544a2afa59af463b8eeacb0c74ea1f0c0bbd0638ff59fabf139a29de1d1f54993b8a8b8b445869f81d8b743a3f0bd94b662b5bc82aec267b0acffb9d867b7ecbbbaf56aab96aaadc8498aad8695b66782ebaad7e259b387af87d32587e7af82d948f5bbb98cd139b1adae89f3489be89da6dc37e2a3; JSESSIONID-WYYY=ouIrq47vC%5Cg1%2FVDcW%2BBlGFBjzZbGRuSPeuv8f29gHncj9FHguZauB2pw0phTig%2Fu%2FGNim7Hz%2FZYzf0EsXSqGNOToEsNXIboUq7aedraesFRe6eZ39PiIt2uRVeMSIdjeW8Nf0C%2FMuGm5mbWf9HsRTK%2BGwvIfAb3EcCcdl%2Fe9FM5237th%3A1699401563876");
        Client.DefaultRequestHeaders.Add("Cookie", "osver=undefined; deviceId=undefined; appver=8.10.05; versioncode=140; mobilename=undefined; buildver=1699788225; resolution=1920x1080; __csrf=; os=android; channel=undefined; requestId=1699788225483_0541;MUSIC_U=00DCE90E4414477F6CEB1D678986B3E798756DC0B3789AC24E863D0F1CDA8392E2191A215A72C87DD76D33AC15963F1D4581ADFAD71698E9CEE1F59205B30465327BD608B9A4C03E907A43561CC8BD9A21C0D400237A879F6E5CDEFED2B7ADD78FD44F6402E41100966CD15F655BE0C37A18D1103134FAAE42BE3D77AEB60D300BE1A2789E1B4F7EB956E1969D2CED89D57D629398263FB44214E8BF12D201B368A9DFF0B1AE062C24A80C57953E8D42B4FBDA2B11ADD2E8C87F230727EAB2D75DC85C3A8D033CF2ABD045131969431DF3BCC689B902402FF9A683CDF5C96EFF1FBFD2563BF50EDAFB2200C887A51F4FF10B4D14A5AA745BBD62DD7DB1C5EA183E3FE575795096A830BF3FA91D685B96E981718C1568BF95E2D9A146509FE4430570AF16B22DC144D77C61D654F90046F61DC210814E63661061EFA80136272A0DF51F97529AC412523D009391B77DAF29");
    }

    /// <summary>
    /// AES加密
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    private string EncryptByAES(string plainText, string key, string iv)
    {
        var Key = Encoding.UTF8.GetBytes(key);
        var IV = Encoding.UTF8.GetBytes(iv);
        byte[] encrypted;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
            encrypted = msEncrypt.ToArray();
        }
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// 生成表单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    private async Task<string> GetPostForm(string url, object param)
    {
        var str = EncryptByAES(JsonConvert.SerializeObject(param), ig, key);
        var args = EncryptByAES(str, iv, key);
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "params", args },
            { "encSecKey", encSecKey }

        });
        var response = await Client.PostAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 通过id获取music链接
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<string> GetMusicUrlByid(long id)
    {
        try
        {
            var urls = await GetMusicUrlByid([id]);
            if (urls.TryGetValue(id, out var url))
                return url;
            else
                throw new Exception("无法获取歌曲url链接");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    /// <summary>
    /// 通过id列表获取音乐链接
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Dictionary<long, string>> GetMusicUrlByid(IEnumerable<long> ids)
    {
        var param = new
        {
            ids = $"[{string.Join(",", ids)}]",
            level = "standard",
            encodeType = "acc",
            csrf_token = ""
        };
        var result = await GetPostForm(playMusic, param);
        var musicData = (JObject)JsonConvert.DeserializeObject(result)!;
        if (string.IsNullOrEmpty(result) || musicData == null)
            throw new Exception("无法获取到数据，请检查Music。加密算法是否更改!");
        if (musicData.Value<int>("code") != 200)
        {
            throw new Exception("API请求失败，无法获取音乐信息!");
        }
        var songs = musicData.Value<JArray>("data")!;
        var res = new Dictionary<long, string>();

        for (int i = 0; i < songs.Count; i++)
        {
            var id = songs[i].Value<int>("id")!;
            var url = songs[i].Value<string>("url")!;
            res.Add(id, url);
        }
        return res;
    }

    /// <summary>
    /// 通过音乐名称和序号获取音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<MusicData> GetMusic(string name, int index)
    {
        index = index <= 0 ? 0 : index - 1;
        var musicList = await GetMusicListByName(name);
        if (musicList.Count == 0)
        {
            throw new Exception("无法获取音乐列表!");
        }

        if (index >= musicList.Count)
        {
            throw new Exception("不存在次序号的音乐!");
        }
        var music = musicList[index];
        music.SetMusicUrl(await GetMusicUrlByid(music.ID));
        await GetMusicByID(music.ID);
        return music;
    }

    /// <summary>
    /// 通过音乐id获取音乐
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<MusicData> GetMusicByID(long id)
    {
        try
        {
            var list = await GetMusicByID([id]);
            if (list.Count == 0)
            {
                throw new Exception("歌曲信息获取失败");
            }
            return list[0];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// 通过音乐id列表获取音乐
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<MusicData>> GetMusicByID(IEnumerable<long> ids)
    {
        var args = new List<object>();
        foreach (var id in ids)
        {
            args.Add(new { id });
        }
        var param = new
        {
            id = ids.First(),
            c = JsonConvert.SerializeObject(args),
            csrf_token = ""
        };
        var result = await GetPostForm(musicinfo, param);
        var data = (JObject)JsonConvert.DeserializeObject(result)!;
        if (string.IsNullOrEmpty(result) || data == null)
            throw new Exception("无法获取到数据，请检查Music。加密算法是否更改!");
        var res = new List<MusicData>();
        if (data.Value<int>("code") == 200)
        {
            var musicList = data.Value<JArray>("songs")!;
            for (int i = 0; i < musicList.Count; i++)
            {
                var musicName = musicList[i].Value<string>("name")!;
                var musicPicurl = musicList[i].Value<JObject>("al")!.Value<string>("picUrl")!;
                var musicID = musicList[i].Value<long>("id")!;
                var musicSingers = musicList[i].Value<JArray>("ar")!;
                var singers = new List<string>();
                var url = await GetMusicUrlByid(musicID);
                for (int n = 0; n < musicSingers.Count; n++)
                {
                    singers.Add(musicSingers[n].Value<string>("name")!);
                }
                res.Add(new(musicName, musicID, musicPicurl, singers, url));
            }
        }
        else
        {
            throw new Exception("无法获取API数据!");
        }
        return res;
    }

    /// <summary>
    /// 通过名称获取音乐列表
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<List<MusicData>> GetMusicListByName(string name)
    {
        var param = new
        {
            hlpretag = "<span class=\"s-fc7\">",
            hlposttag = "</span>",
            s = name,
            type = 1,
            offset = 0,
            total = "true",
            limit = 30,
            csrf_token = ""
        };
        var result = await GetPostForm(musicList, param);
        var data = (JObject)JsonConvert.DeserializeObject(result)!;
        if (string.IsNullOrEmpty(result) || data == null)
            throw new Exception("无法获取到数据，请检查Music_163。加密算法是否更改!");
        var res = new List<MusicData>();
        if (data.Value<int>("code") == 200)
        {
            var musicList = data.Value<JObject>("result")!.Value<JArray>("songs")!;
            for (int i = 0; i < musicList.Count; i++)
            {
                var musicName = musicList[i].Value<string>("name")!;
                var musicPicurl = musicList[i].Value<JObject>("al")!.Value<string>("picUrl")!;
                var musicID = musicList[i].Value<long>("id")!;
                var musicSingers = musicList[i].Value<JArray>("ar")!;
                var singers = new List<string>();
                for (int n = 0; n < musicSingers.Count; n++)
                {
                    singers.Add(musicSingers[n].Value<string>("name")!);
                }
                res.Add(new(musicName, musicID, musicPicurl, singers));
            }
        }
        else
        {
            throw new Exception("无法获取API数据!");
        }
        return res;
    }
}

