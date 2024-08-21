
using ProtoBuf;

namespace MorMor.Model.Socket.PlayerMessage;

[ProtoContract]
public class PlayerChatMessage : BasePlayerMessage
{
    [ProtoMember(8)] public string Text { get; set; } = string.Empty;

    [ProtoMember(9)] public byte[] Color { get; set; } = new byte[3];
}
