using ProtoBuf;

namespace MorMor.Model.Terraria;

[ProtoContract]
public class PlayerInfo
{
    [ProtoMember(1)] public int Index { get; set; }

    [ProtoMember(2)] public string Name { get; set; }

    [ProtoMember(3)] public string Group { get; set; }
}
