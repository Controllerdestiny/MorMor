using MomoAPI.Entities;
using MomoAPI.Entities.Info;
using MomoAPI.Model.Event.MessageEvent;

namespace MomoAPI.EventArgs;

public class GroupMessageEventArgs : MessageEventArgs
{
    internal GroupMessageEventArgs(OnebotGroupMsgEventArgs args) : base(args)
    {
        Group = new(args.GroupId);
        IsSuperAdmin = args.SenderInfo.Role == Enumeration.EventParamType.MemberRoleType.Admin || args.SenderInfo.Role == Enumeration.EventParamType.MemberRoleType.Owner;
        SenderInfo = args.SenderInfo;
    }

    public Group Group { get; }

    public GroupSenderInfo SenderInfo { get; }

    public bool IsSuperAdmin { get; }

    public async Task<(ApiStatus, long)> Reply(MessageBody body)
    { 
        return await Group.Reply(body);
    }
}
