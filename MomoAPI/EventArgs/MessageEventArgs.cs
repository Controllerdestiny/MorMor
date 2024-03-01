using MomoAPI.Converter;
using MomoAPI.Entities;
using MomoAPI.Model.Event.MessageEvent;

namespace MomoAPI.EventArgs;

public class MessageEventArgs : BaseMomoEventArgs
{
    /// <summary>
    /// 消息体
    /// </summary>
    public MessageContext MessageContext { get; }

    /// <summary>
    /// 发送者
    /// </summary>
    public Sender Sender { get; }

    /// <summary>
    /// 是否为机器人本身的消息
    /// </summary>
    public bool IsSelfMessage { get; }

    public long SelfId { get; }

    internal MessageEventArgs(BaseObMessageEventArgs args)
    {
        MessageContext = new MessageContext(args.Font, TimeSpan.FromMilliseconds(args.Time), args.MessageList.ToMessageBody(), args.RawMessage, args.MessageId);
        Sender = new(args.UserId);
        IsSelfMessage = args.UserId == args.SelfId;
        SelfId = args.SelfId;
    }
}
