using ProtoBuf;

namespace MorMor.Model.Socket.Action.Receive;

[ProtoContract]
public class ExportPlayerArgs : BaseAction
{
    [ProtoMember(5)] public List<string> Names { get; set; } = [];
}
