
using System.Text;

namespace MorMor.Music;


public class MusicTool
{
    private static readonly Dictionary<long, string> MusicLocal = new();
    private static readonly Dictionary<long, string> MusicName = new();

    public static async Task<List<QQ.MusicInfo>> GetMusicQQList(string musicName)
    {
        return await QQ.MusicQQ.GetMusicList(musicName);
    }

    public static async Task<string> QQMusic(string musicName)
    {
        var list = await GetMusicQQList(musicName);
        string ret = "";
        int i = 1;
        list.ForEach(x =>
        {
            ret += $"[{i}].{x.song} -- {x.singer}\n";
            i++;
        });
        ret += "资源来自于QQ音乐";
        return ret;
    }

    public static async Task<string> GetMusicQQMarkdown(string musicName)
    {
        var list = await GetMusicQQList(musicName);
        var sb = new StringBuilder($$"""<div align="center">""");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("# QQ音乐");
        for (int i = 0; i < list.Count; i++)
        {
            sb.AppendLine($"## `{i + 1}`- {list[i].song} -- {list[i].singer}");
        }
        sb.AppendLine();
        sb.AppendLine($$"""</div>""");
        return sb.ToString();
    }

    public static async Task<string> GetMusic163Markdown(string musicName)
    {
        var list = await GetMusic163List(musicName);
        var sb = new StringBuilder($$"""<div align="center">""");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("# 网易云音乐");
        for (int i = 0; i < list.Count; i++)
        {
            sb.AppendLine($"## `{i + 1}`- {list[i].name} -- {string.Join(",", list[i].singers.Select(x => x.name))}");
        }
        sb.AppendLine();
        sb.AppendLine($$"""</div>""");
        return sb.ToString();
    }

    public static async Task<string> WangYiMusic(string musicName)
    {
        var list = await GetMusic163List(musicName);
        string ret = "";
        int i = 1;
        list.ForEach(x =>
        {
            ret += $"[{i}].{x.name} -- {string.Join(",", x.singers.Select(x => x.name))}\n";
            i++;
        });
        ret += "资源来自于网易音乐";
        return ret;

    }

    public static async Task<List<_163.MusicInfo>> GetMusic163List(string musicName)
    {
        return await _163.Music163.GetMusicList(musicName);
    }

    public static async Task<QQ.MusicData?> GetMusicQQ(string musicName, int index)
    {
        return await QQ.MusicQQ.GetMusic(musicName, index);
    }

    public static async Task<_163.MusicData?> GetMusic163(string musicName, int index)
    {
        return await _163.Music163.GetMusic(musicName, index);
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
        if (MusicLocal.TryGetValue(uin, out var music))
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
