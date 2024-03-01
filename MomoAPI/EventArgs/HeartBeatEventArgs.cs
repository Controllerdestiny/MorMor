using MomoAPI.Model.Event.MetaEvent;
using Newtonsoft.Json.Linq;

namespace MomoAPI.EventArgs;

public class HeartBeatEventArgs : BaseMomoEventArgs
{
    public JObject Status { get; }

    public int Interval { get; }

    public long SelfId { get; }

    internal HeartBeatEventArgs(OnebotHeartBeatEventArgs args)
    {
        Status = args.Status;
        Interval = args.Interval;
        SelfId = args.SelfId;
    }
}
