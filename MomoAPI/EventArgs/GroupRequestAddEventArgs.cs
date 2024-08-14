using MomoAPI.Adapter;
using MomoAPI.Entities;
using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Model.Event.RequestEvent;

namespace MomoAPI.EventArgs;

public class GroupRequestAddEventArgs : BaseMomoEventArgs
{
    public Group Group { get; }

    public string Flag { get; }

    public string Comment { get; }

    public Sender User { get; }

    public GroupRequestType RequestType { get; }

    internal GroupRequestAddEventArgs(OneBotGroupRequestAddEventArgs args)
    {
        User = new(args.UID);
        Group = new(args.GroupId);
        Flag = args.Flag;
        Comment = args.Comment;
        RequestType = args.RequestType;
    }

    public async ValueTask<ApiStatus> SetGroupAddReuqest(bool approve, string remark = "")
    {
        return await ApiAdapter.SetGroupAddRequest(Flag, approve, remark);
    }
}
