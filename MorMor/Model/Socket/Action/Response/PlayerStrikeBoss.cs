using MorMor.Model.Socket.Internet;
using ProtoBuf;


namespace MorMor.Model.Socket.Action.Response;

[ProtoContract]
public class PlayerStrikeBoss : BaseActionResponse
{
    [ProtoMember(8)] public List<KillNpc> Damages { get; set; }
}
