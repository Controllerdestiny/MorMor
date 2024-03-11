using MomoAPI.Entities;
using MomoAPI.Model.Event.NoticeEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomoAPI.EventArgs;

public class GroupUnMuteEventArgs : BaseMomoEventArgs
{
    public Sender Operator { get; }

    public Sender Target { get; }

    public Group Group { get; }

    internal GroupUnMuteEventArgs(OneBotGroupMuteEventArgs args)
    {
        Operator = new Sender(args.OperatorId);
        Target = new Sender(args.TargetId);
        Group = new Group(args.GroupId);
    }
}
