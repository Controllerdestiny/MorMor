using ProtoBuf;

namespace MorMor.Model.Socket.Internet;

[ProtoContract]
public class PlayerInfo
{
    [ProtoMember(1)] public int Index { get; set; }

    [ProtoMember(2)] public string Name { get; set; } = string.Empty;

    [ProtoMember(3)] public string Group { get; set; } = string.Empty;

    [ProtoMember(4)] public string Prefix { get; set; } = string.Empty;

    [ProtoMember(5)] public bool IsLogin { get; set; }

}
