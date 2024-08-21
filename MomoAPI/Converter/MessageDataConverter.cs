using MomoAPI.Entities;
using MomoAPI.Entities.Segment;
using MomoAPI.Entities.Segment.DataModel;
using MomoAPI.Enumeration;
using MomoAPI.Extensions;
using MomoAPI.Model.API;
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
            if (onebotSegment.RawData.Count == 0)
                return new MomoSegment(SegmentType.Unknown, new());
            return onebotSegment.MsgType switch
            {
                SegmentType.Text => new MomoSegment(SegmentType.Text, onebotSegment.RawData.ToObject<Text>()!),
                SegmentType.Face => new MomoSegment(SegmentType.Face, onebotSegment.RawData.ToObject<Face>()!),
                SegmentType.Image => new MomoSegment(SegmentType.Image, onebotSegment.RawData.ToObject<Image>()!),
                SegmentType.Record => new MomoSegment(SegmentType.Record, onebotSegment.RawData.ToObject<Record>()!),
                SegmentType.At => new MomoSegment(SegmentType.At, onebotSegment.RawData.ToObject<At>()!),
                SegmentType.Reply => new MomoSegment(SegmentType.Reply, onebotSegment.RawData.ToObject<Reply>()!),
                SegmentType.Json => new MomoSegment(SegmentType.Json, onebotSegment.RawData.ToObject<Json>()!),
                SegmentType.File => new MomoSegment(SegmentType.File, onebotSegment.RawData.ToObject<File>()!),
                SegmentType.Video => new MomoSegment(SegmentType.Video, onebotSegment.RawData.ToObject<Video>()!),
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