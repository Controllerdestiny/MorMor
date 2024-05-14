using ProtoBuf;

namespace MorMor.Model.Socket.Action.Response;

[ProtoContract]
public class ServerCommand : BaseActionResponse
{
    [ProtoMember(8)] public List<string> Params { get; set; } = new();

}
