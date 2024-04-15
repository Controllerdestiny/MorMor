using Microsoft.Win32.SafeHandles;
using MomoAPI.Entities.Segment.DataModel;
using MomoAPI.Enumeration;
using MomoAPI.Model.API;
using MomoAPI.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Xml.Linq;

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

    public static MomoSegment Image(string file)
    {
        return new MomoSegment(SegmentType.Image, new Image()
        {
            File = file
        });
    }

    public static MomoSegment Image(Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        var base64 = Convert.ToBase64String(bytes);
        return new MomoSegment(SegmentType.Image, new Image()
        {
            File = "base64://" + base64
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

    public static MomoSegment File(string data, string name = null)
    {
        return new MomoSegment(SegmentType.File, new DataModel.File()
        {
            Data = data,
            Name = name
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

    public static MomoSegment Json(string data)
    {
        return new MomoSegment(SegmentType.Json, new Json()
        {
            Connect = data
        });
    }

    public static MomoSegment Music(string type, string musicUrl, string picUlr, string song, string singer)
    {
        var url = $"https://api.xn--7gqa009h.top/api/yyArk?url={HttpUtility.UrlEncode(musicUrl)}&pic={picUlr}&name={song}&title={singer}&type={type}";
        HttpClient client = new();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:124.0) Gecko/20100101 Firefox/124.0");
        client.DefaultRequestHeaders.Add("Cookie", "cf_clearance=gCzdnfowPu_Jp05_pZaUf5ESD7QAcMeb_O_luXy0F78-1713174342-1.0.1.1-sZ1nXhYRjNdpmUD18YYKHA3cRMKDJLjTX4q90E54bO3O.0GDLiGvNRlpP21UWJhEOkk2bgnAxRx6pnjxpWESUw; myhk_player_switch=no; myhk_player_album=0; myhk_player_song=0; Hm_lvt_90f8c558cb9b46cac96d0fb252e0032f=1713169595,1713174341; notice=1; cf_chl_rc_m=3; cf_chl_3=6af6fe52d761790");
        try
        {
            var Context = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url)).Result;
            var data =  Context.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(data);
            return Json(data);
          
        }
        catch (Exception ex)
        {
            return Text(ex.Message);
        }
    }

    public static MomoSegment Music_QQ(string musicUrl, string picUlr, string song, string singer)
    {
        return Music("QQ音乐", musicUrl, picUlr, song, singer);
    }

    public static MomoSegment Music_163(string musicUrl, string picUlr, string song, string singer)
    {
        return Music("网易云音乐", musicUrl, picUlr, song, singer);
    }

    public static implicit operator MomoSegment(string text)
    {
        return Text(text);
    }
}
