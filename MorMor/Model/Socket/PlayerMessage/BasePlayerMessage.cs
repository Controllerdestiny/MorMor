
using ProtoBuf;

namespace MorMor.Model.Socket.PlayerMessage;

[ProtoContract]
[ProtoInclude(201, typeof(PlayerChatMessage))]
[ProtoInclude(202, typeof(PlayerCommandMessage))]
[ProtoInclude(203, typeof(PlayerJoinMessage))]
[ProtoInclude(204, typeof(PlayerLeaveMessage))]
public class BasePlayerMessage : BaseMessage
{
    [ProtoMember(4)] public string Name { get; set; } = string.Empty;

    [ProtoMember(5)] public string Group { get; set; } = string.Empty;

    [ProtoMember(6)] public string Prefix { get; set; } = string.Empty; 

    [ProtoMember(7)] public bool IsLogin { get; set; }

    [ProtoMember(8)] public bool Mute { get; set; }

}
