namespace MomoAPI.Music._163;

public class MusicData
{
    public string Name { get; }

    public long ID { get; }

    public string Picture { get; }

    public List<string> Singers { get; }

    public string JumpUrl { get; }

    public string MusicUrl { get; private set; } = string.Empty;

    public MusicData(string name, long iD, string picUrl, List<string> singers, string musicUrl = "")
    {
        Name = name;
        ID = iD;
        Picture = picUrl;
        Singers = singers;
        JumpUrl = $"https://music.163.com/#/song?id={ID}";
        MusicUrl = musicUrl;
    }

    internal void SetMusicUrl(string url)
    {
        MusicUrl = url;
    }
}
