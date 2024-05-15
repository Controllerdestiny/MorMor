using MomoAPI.Entities.Segment;
using MomoAPI.Enumeration;
using Newtonsoft.Json;
using System.Collections;

namespace MomoAPI.Entities;

public class MessageBody : IList<MomoSegment>
{
    /// <summary>
    /// 消息列表
    /// 序列化时用得到
    /// </summary>
    [JsonProperty("message")]
    private readonly List<MomoSegment> _messages = new();

    public MomoSegment this[int index] { get => ((IList<MomoSegment>)_messages)[index]; set => ((IList<MomoSegment>)_messages)[index] = value; }

    public int Count => _messages.Count;

    public bool IsReadOnly => false;

    public MessageBody(List<MomoSegment> momoSegments)
    {
        _messages = momoSegments;
    }

    public MessageBody() : base()
    {

    }

    public MessageBody Text(string text)
    {
        Add(MomoSegment.Text(text));
        return this;
    }

    public MessageBody Image(string file)
    {
        Add(MomoSegment.Image(file));
        return this;
    }

    public MessageBody Image(Stream stream)
    {
        Add(MomoSegment.Image(stream));
        return this;
    }

    public MessageBody Reply(long msgId)
    {
        Add(MomoSegment.Reply(msgId));
        return this;
    }

    public MessageBody Record(string file)
    {
        Add(MomoSegment.Record(file));
        return this;
    }

    public MessageBody At(long uid)
    {
        Add(MomoSegment.At(uid));
        return this;
    }

    public MessageBody AtAll()
    {
        Add(MomoSegment.AtAll());
        return this;
    }

    public MessageBody Face(int faceid)
    {
        Add(MomoSegment.Face(faceid));
        return this;
    }

    public MessageBody Music(string jumpUrl, string AudioUrl, string imageUrl, string song, string singer)
    {
        Add(MomoSegment.Music(jumpUrl, AudioUrl, imageUrl, song, singer));
        return this;
    }

    public MessageBody Music_QQ(string jumpUrl, string AudioUrl, string imageUrl, string song, string singer)
    {
        Add(MomoSegment.Music_QQ(jumpUrl, AudioUrl, imageUrl, song, singer));
        return this;
    }

    public MessageBody Music_163(string jumpUrl, string AudioUrl, string imageUrl, string song, string singer)
    {
        Add(MomoSegment.Music_163(jumpUrl, AudioUrl, imageUrl, song, singer));
        return this;
    }

    public MessageBody CustomMusic(MusicType type, string jumpUrl, string AudioUrl, string imageUrl, string song, string singer)
    {
        Add(MomoSegment.CustomMusic(type, jumpUrl, AudioUrl, imageUrl, song, singer));
        return this;
    }

    public MessageBody File(string file, string name = null)
    {
        Add(MomoSegment.File(file, name));
        return this;
    }

    public MessageBody Video(string file)
    {
        Add(MomoSegment.Video(file));
        return this;
    }

    public MessageBody Json(string data)
    {
        Add(MomoSegment.Json(data));
        return this;
    }

    public void Add(MomoSegment item)
    {
        _messages.Add(item);
    }

    public void Clear()
    {
        _messages.Clear();
    }

    public bool Contains(MomoSegment item)
    {
        return _messages.Contains(item);
    }

    public void CopyTo(MomoSegment[] array, int arrayIndex)
    {
        _messages.CopyTo(array, arrayIndex);
    }

    public IEnumerator<MomoSegment> GetEnumerator()
    {
        return _messages.GetEnumerator();
    }

    public int IndexOf(MomoSegment item)
    {
        return _messages.IndexOf(item);
    }

    public void Insert(int index, MomoSegment item)
    {
        _messages.Insert(index, item);
    }

    public bool Remove(MomoSegment item)
    {
        return _messages.Remove(item);
    }

    public void RemoveAt(int index)
    {
        _messages.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _messages.GetEnumerator();
    }

    public static implicit operator MessageBody(string text)
    {
        return new MessageBody() { text };
    }
}
