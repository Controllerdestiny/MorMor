using MomoAPI.Entities.Info;
using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Model.Event.MessageEvent;

namespace MomoAPI.EventArgs;

public class PrivateMessageEventArgs : MessageEventArgs
{
    public PrivateSenderInfo SenderInfo { get; }

    public bool IsTemporaryMessage { get; }

    internal PrivateMessageEventArgs(OnebotPrivateMsgEventArgs args) : base(args)
    {
        SenderInfo = args.SenderInfo;
        IsTemporaryMessage = args.SubType == SubType.Group;
    }
}
