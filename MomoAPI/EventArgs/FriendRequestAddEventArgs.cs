using MomoAPI.Adapter;
using MomoAPI.Entities;
using MomoAPI.Model.Event.RequestEvent;

namespace MomoAPI.EventArgs;

public class FriendRequestAddEventArgs : BaseMomoEventArgs
{
    public Sender User { get; }

    public string Flag { get; }

    public string Comment { get; }

    internal FriendRequestAddEventArgs(OneBotFriendRequestAddEventArgs args)
    {
        User = new(args.UID);
        Flag = args.Flag;
        Comment = args.Comment;
    }

    public async Task<ApiStatus> SetFriendAddRequest(bool approve, string remark = "")
    {
        return await ApiAdapter.SetFriendAddRequest(Flag, approve, remark);
    }
}
