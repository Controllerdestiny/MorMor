using ProtoBuf;

namespace MorMor.Model.Socket.Action.Receive;

[ProtoContract]
public class ServerCommandArgs : BaseAction
{
    [ProtoMember(5)] public string Text { get; set; }
}
