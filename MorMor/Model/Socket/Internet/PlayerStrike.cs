using ProtoBuf;

namespace MorMor.Model.Socket.Internet;

[ProtoContract]
public class PlayerStrike
{
    [ProtoMember(1)] public string Player { get; set; }

    [ProtoMember(2)] public int Damage { get; set; }
}
