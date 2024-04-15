using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MorMor.Music.Music_QQ;
using MorMor.Music.Music_163;
using System.Xml.Linq;

namespace MorMor.Music;


public class MusicTool
{
    private static Dictionary<long, string> MusicLocal = new();
    private static Dictionary<long, string> MusicName = new();

    public static async Task<List<QQMusicData>> GetMusicQQList(string musicName)
    {
        var music = new Music_QQ.Music_QQ();
        return await music.GetMusicListByName(musicName, new QQCookie());
    }

    public static async Task<string> QQMusic(string musicName)
    {
        var list = await GetMusicQQList(musicName);
        string ret = "";
        int i = 1;
        list.ForEach(x =>
        {
            ret += $"[{i}].{x.Name} -- {string.Join(",", x.Singers)}\n";
            i++;
        });
        ret += "资源来自于QQ音乐";
        return ret;
    }

    public static async Task<string> WangYiMusic(string musicName)
    {
        var list = await GetMusic163List(musicName);
        string ret = "";
        int i = 1;
        list.ForEach(x =>
        {
            ret += $"[{i}].{x.Name} -- {string.Join(",", x.Singers)}\n";
            i++;
        });
        ret += "资源来自于网易音乐";
        return ret;

    }

    public static async Task<List<MusicData>> GetMusic163List(string musicName)
    {
        var music = new Music_163.Music_163();
        return await music.GetMusicListByName(musicName);
    }

    public static async Task<QQMusicData> GetMusicQQ(string musicName, int index)
    {
        Music_QQ.Music_QQ music_QQ = new();
        return await music_QQ.GetMusic(new ChoiceMusicSetting()
        {
            Name = musicName,
            Index = index,
            Cookie = new QQCookie(),
            br = 4
        });
    }

    public static async Task<QQMusicData> GetMusic163(string musicName, int index)
    {
        Music_QQ.Music_QQ music_QQ = new();
        return await music_QQ.GetMusic(new ChoiceMusicSetting()
        {
            Name = musicName,
            Index = index,
            Cookie = new QQCookie(),
            br = 4
        });
    }

    public static void ChangeLocal(string local, long uin)
    {
        MusicLocal[uin] = local;
    }
    public static void ChangeName(string name, long uin)
    {
        MusicName[uin] = name;
    }

    public static string GetLocal(long uin)
    {
        if(MusicLocal.TryGetValue(uin, out var music))
        {
            return music;
        }
        return "QQ";
    }

    public static string? GetName(long uin)
    {
        if (MusicName.TryGetValue(uin, out var music))
        {
            return music;
        }
        return null;
    }
}
