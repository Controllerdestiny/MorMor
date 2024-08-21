using MomoAPI.Entities.Info;
using MomoAPI.Model.Event.NoticeEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomoAPI.EventArgs;

public class GroupUpLoadFileEventArgs(OneBotGroupUpLoadFileEventArgs args) : BaseMomoEventArgs
{
    public long GroupId { get; } = args.GroupId;

    public GroupUpLoadInfo UpLoad { get; } = args.UpLoad;

    public long UserId { get; } = args.Uid;
}
