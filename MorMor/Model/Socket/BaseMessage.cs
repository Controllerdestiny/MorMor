using Fleck;
using MorMor.Enumeration;
using MorMor.Model.Socket.Action;
using MorMor.Model.Socket.PlayerMessage;
using MorMor.Model.Socket.ServerMessage;
using MorMor.Terraria;
using ProtoBuf;

namespace MorMor.Model.Socket;

[ProtoContract]
[ProtoInclude(101, typeof(BasePlayerMessage))]
[ProtoInclude(102, typeof(BaseAction))]
[ProtoInclude(103, typeof(GameInitMessage))]
public class BaseMessage
{
    [ProtoMember(1)] public PostMessageType MessageType { get; set; }

    [ProtoMember(2)] public string ServerName { get; set; }

    [ProtoMember(3)] public string Token { get; set; } = string.Empty;

    public bool Handler { get; set; }

    public IWebSocketConnection Client { get; set; }

    public TerrariaServer? TerrariaServer => MorMorAPI.Setting.GetServer(ServerName);
}
