using MorMor.Enumeration;
using ProtoBuf;

namespace MorMor.Model.Socket.Action.Receive;

[ProtoContract]
public class SocketConnectStatusArgs : BaseAction
{
    [ProtoMember(5)] public SocketConnentType Status { get; set; }
}
