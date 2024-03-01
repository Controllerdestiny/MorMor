using MomoAPI.Model.Event.NoticeEvent;

namespace MomoAPI.EventArgs;

public class FriendRecallEventArgs : BaseMomoEventArgs
{
    public long UID { get; }

    public long MessageID { get; }

    internal FriendRecallEventArgs(OneBotFriendRecallEventArgs args)
    {
        UID = args.UID;
        MessageID = args.MessageId;
    }
}
