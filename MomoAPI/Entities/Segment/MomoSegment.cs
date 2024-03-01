using MomoAPI.Entities.Segment.DataModel;
using MomoAPI.Enumeration;
using MomoAPI.Model.API;
using MomoAPI.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MomoAPI.Entities.Segment;

public class MomoSegment
{
    [JsonProperty("type")]
    [JsonConverter(typeof(EnumConverter))]
    public SegmentType Type { get; set; }

    [JsonProperty("data")]
    public BaseMessage MessageData { get; set; }

    public MomoSegment(SegmentType type, BaseMessage messageData)
    {
        Type = type;
        MessageData = messageData;
    }

    internal OnebotSegment ToOnebotSegment()
    {
        return new OnebotSegment
        {
            MsgType = Type,
            RawData = JObject.FromObject(MessageData)
        };
    }

    public static MomoSegment Text(string text)
    {
        return new MomoSegment(SegmentType.Text, new Text()
        {
            Content = text
        });
    }

    public static MomoSegment Image(string url)
    {
        return new MomoSegment(SegmentType.Image, new Image()
        {
            File = url
        });
    }

    public static MomoSegment Image(Stream stream)
    {
        using var memory = new MemoryStream();
        memory.CopyTo(stream);
        var base64 = Convert.ToBase64String(memory.ToArray());
        return new MomoSegment(SegmentType.Image, new Image()
        {
            File = base64
        });
    }

    public static MomoSegment At(long qq)
    {
        return new MomoSegment(SegmentType.At, new At()
        {
            Target = qq.ToString()
        });
    }

    public static MomoSegment Face(int id)
    {
        return new MomoSegment(SegmentType.Face, new Face()
        {
            Id = id
        });
    }

    public static MomoSegment Reply(long messageid)
    {
        return new MomoSegment(SegmentType.Reply, new Reply()
        {
            Uid = messageid,
        });
    }

    public static MomoSegment AtAll()
    {
        return new MomoSegment(SegmentType.At, new At()
        {
            Target = "all",
        });
    }

    public static MomoSegment File(string data)
    {
        return new MomoSegment(SegmentType.File, new DataModel.File()
        {
            Data = data
        });
    }

    public static MomoSegment Video(string data)
    {
        return new MomoSegment(SegmentType.Video, new DataModel.Video()
        {
            Data = data
        });
    }

    public static MomoSegment Record(string data)
    {
        return new MomoSegment(SegmentType.Record, new Record()
        {
            File = data
        });
    }

    public static implicit operator MomoSegment(string text)
    {
        return Text(text);
    }
}
