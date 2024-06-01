using ProtoBuf;

namespace MorMor.Model.Socket.Internet;

[ProtoContract]
public class KillNpc
{
    [ProtoMember(1)] public int Id { get; set; }

    [ProtoMember(2)] public int MaxLife { get; set; }

    [ProtoMember(3)] public string Name { get; set; } = "";

    [ProtoMember(4)] public List<PlayerStrike> Strikes { get; set; } = new();

    [ProtoMember(5)] public DateTime KillTime { get; set; }

    [ProtoMember(6)] public DateTime SpawnTime { get; set; }

    [ProtoMember(7)] public bool IsAlive { get; set; }
}



