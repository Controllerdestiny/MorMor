using MomoAPI.Model.Event.MetaEvent;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MomoAPI.EventArgs;

public class HeartBeatEventArgs : BaseMomoEventArgs
{
    public JsonObject Status { get; }

    public int Interval { get; }

    public long SelfId { get; }

    internal HeartBeatEventArgs(OnebotHeartBeatEventArgs args)
    {
        Status = args.Status;
        Interval = args.Interval;
        SelfId = args.SelfId;
    }
}
