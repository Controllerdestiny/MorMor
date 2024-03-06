﻿using MomoAPI.Entities.Segment.DataModel;
using MomoAPI.Utils;

namespace MomoAPI.Entities;

public class MessageContext
{
    public int Font { get; init; }

    public TimeSpan TimeSpan { get; init; }

    public MessageBody Messages { get; init; }

    public string RawText { get; init; }

    public long MessageID { get; init; }

    public MessageContext(int font, TimeSpan timeSpan, MessageBody messages, string rawText, long messageID)
    {
        Font = font;
        TimeSpan = timeSpan;
        Messages = messages;
        RawText = rawText;
        MessageID = messageID;
    }

    public string GetText()
    {
        string text = string.Empty;
        Messages.ForEach(x =>
        {
            if (x.Type == Enumeration.SegmentType.Text)
            {
                text += (x.MessageData as Text)?.Content;
            }
        });
        return text;
    }

    public List<Image> GetImages()
    {
        return Messages.Where(msg => msg.Type == Enumeration.SegmentType.Image).Select(img => img.MessageData as Image).ToList();
    }

    public List<At> GetAts()
    {
        return Messages.Where(msg => msg.Type == Enumeration.SegmentType.At).Select(img => img.MessageData as At).ToList();
    }
}
