using MomoAPI.Entities;
using MomoAPI.Model.Event.NoticeEvent;

namespace MomoAPI.EventArgs;

public class FriendAddEventArgs : BaseMomoEventArgs
{
    public Sender Sender { get; }

    internal FriendAddEventArgs(OneBotFriendAddEventArgs args)
    {
        Sender = new(args.Uid);
    }
}
