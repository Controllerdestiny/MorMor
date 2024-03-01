using MomoAPI.Entities;
using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Model.Event.NoticeEvent;

namespace MomoAPI.EventArgs;

public class GroupMemberChangeEventArgs : BaseMomoEventArgs
{
    public Sender ChangeUser { get; }

    public Sender Operator { get; }

    public MemberChangeType ChangeType { get; }

    public NoticeType NoticeType { get; }

    public Group Group { get; }
    internal GroupMemberChangeEventArgs(OneBotGroupMemberChangeEventArgs args)
    {
        ChangeType = args.ChangeType;
        ChangeUser = new(args.Uid);
        Operator = new(args.OperatorId);
        NoticeType = args.NoticeType;
        Group = new(args.GroupId);
    }
}
