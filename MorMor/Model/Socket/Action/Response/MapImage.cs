using ProtoBuf;


namespace MorMor.Model.Socket.Action.Response;

[ProtoContract]
public class MapImage : BaseActionResponse
{
    [ProtoMember(8)] public byte[] Buffer { get; set; }
}
