using MorMor.Model.Socket.Internet;
using ProtoBuf;

namespace MorMor.Model.Socket.Action.Response;

[ProtoContract]
public class ServerOnline : BaseActionResponse
{
    [ProtoMember(8)] public List<PlayerInfo> Players { get; set; } = new();

    [ProtoMember(9)] public int MaxCount { get; set; }

    [ProtoMember(10)] public int OnlineCount { get; set; }
}
