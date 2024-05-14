using MorMor.Enumeration;
using ProtoBuf;

namespace MorMor.Model.Socket.Action.Receive;

[ProtoContract]
public class MapImageArgs : BaseAction
{
    [ProtoMember(5)] public ImageType ImageType { get; set; }
}
