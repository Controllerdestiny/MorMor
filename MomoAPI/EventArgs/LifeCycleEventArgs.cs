using MomoAPI.Model.Event.MetaEvent;

namespace MomoAPI.EventArgs;

public class LifeCycleEventArgs : BaseMomoEventArgs
{
    public string SubType { get; }

    public long SelfId { get; }

    internal LifeCycleEventArgs(OneBotLifeCycleEventArgs args)
    {
        SelfId = args.SelfId;
        SubType = args.SubType;
    }
}
