using MomoAPI.Entities;
using MomoAPI.Model.Event.NoticeEvent;

namespace MomoAPI.EventArgs;

public class GroupMuteEventArgs : BaseMomoEventArgs
{
    public Sender Operator { get; }

    public Sender Target { get; }

    public Group Group { get; }

    public int Duration { get; }

    internal GroupMuteEventArgs(OneBotGroupMuteEventArgs args)
    {
        Operator = new Sender(args.OperatorId);
        Target = new Sender(args.TargetId);
        Group = new Group(args.GroupId);
        Duration = args.Duration;
    }
}
