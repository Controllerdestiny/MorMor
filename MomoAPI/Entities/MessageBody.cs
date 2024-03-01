using MomoAPI.Entities.Segment;
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
        Insert(index, item);
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
