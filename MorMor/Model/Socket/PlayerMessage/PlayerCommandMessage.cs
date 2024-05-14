
using ProtoBuf;

namespace MorMor.Model.Socket.PlayerMessage;

[ProtoContract]
public class PlayerCommandMessage : BasePlayerMessage
{
    [ProtoMember(8)] public string Command { get; set; }

    [ProtoMember(9)] public string CommandPrefix { get; set; }
}
