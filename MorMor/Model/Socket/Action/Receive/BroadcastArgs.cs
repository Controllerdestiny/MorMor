using ProtoBuf;

namespace MorMor.Model.Socket.Action.Receive;


[ProtoContract]
[ProtoInclude(501, typeof(PrivatMsgArgs))]
public class BroadcastArgs : BaseAction
{
    [ProtoMember(5)] public string Text { get; set; }

    [ProtoMember(6)] public byte[] Color { get; set; }

}
