using MomoAPI.Entities;
using MomoAPI.Entities.Segment;
using MomoAPI.Entities.Segment.DataModel;
using MomoAPI.Enumeration;
using MomoAPI.Model.API;
using Newtonsoft.Json.Linq;
using File = MomoAPI.Entities.Segment.DataModel.File;

namespace MomoAPI.Converter;

internal static class MessageConverter
{
    #region 上报消息处理

    /// <summary>
    /// 处理接收到的消息段
    /// </summary>
    /// <param name="onebotSegment">消息段</param>
    /// <returns>消息段列表</returns>
    private static MomoSegment ParseMessageElement(OnebotSegment onebotSegment)
    {
        if (onebotSegment.RawData == null)
            return new MomoSegment(SegmentType.Unknown, new());
        try
        {
            JObject jsonObj = JObject.FromObject(onebotSegment.RawData);
            if (jsonObj.Count == 0)
                return new MomoSegment(SegmentType.Unknown, new());
            return onebotSegment.MsgType switch
            {
                SegmentType.Text => new MomoSegment(SegmentType.Text, jsonObj.ToObject<Text>()!),
                SegmentType.Face => new MomoSegment(SegmentType.Face, jsonObj.ToObject<Face>()!),
                SegmentType.Image => new MomoSegment(SegmentType.Image, jsonObj.ToObject<Image>()!),
                SegmentType.Record => new MomoSegment(SegmentType.Record, jsonObj.ToObject<Record>()!),
                SegmentType.At => new MomoSegment(SegmentType.At, jsonObj.ToObject<At>()!),
                SegmentType.Reply => new MomoSegment(SegmentType.Reply, jsonObj.ToObject<Reply>()!),
                SegmentType.Json => new MomoSegment(SegmentType.Json, jsonObj.ToObject<Json>()!),
                SegmentType.File => new MomoSegment(SegmentType.File, jsonObj.ToObject<File>()!),
                SegmentType.Video => new MomoSegment(SegmentType.Video, jsonObj.ToObject<Video>()!),
                _ => new MomoSegment(SegmentType.Unknown, new())
            };
        }
        catch
        {

            return new MomoSegment(SegmentType.Unknown, new());
        }
    }

    /// <summary>
    /// 处理消息段数组
    /// </summary>
    /// <param name="messages">消息段数组</param>
    internal static MessageBody ToMessageBody(this List<OnebotSegment> messages)
    {

        if (messages == null || messages.Count == 0)
            return [];
        List<MomoSegment> retMsg = messages.Select(ParseMessageElement).ToList();


        return new MessageBody(retMsg);
    }

    #endregion
}