using MomoAPI.Entities;
using MomoAPI.Model.Event.NoticeEvent;

namespace MomoAPI.EventArgs;

public class GroupRecallEventArgs : BaseMomoEventArgs
{
    public Group Group { get; }

    public Sender MessageSender { get; }

    public Sender Operator { get; }

    public long MessageID { get; }

    internal GroupRecallEventArgs(OneBotGroupRecallEventArgs args)
    {
        Group = new(args.GroupId);
        MessageSender = new(args.Uid);
        Operator = new(args.OperatorId);
        MessageID = args.MessageId;
    }
}
