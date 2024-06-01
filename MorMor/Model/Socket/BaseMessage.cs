using MorMor.Enumeration;
using MorMor.Model.Socket.Action;
using MorMor.Model.Socket.PlayerMessage;
using MorMor.Model.Socket.ServerMessage;
using MorMor.TShock.Server;
using Newtonsoft.Json;
using ProtoBuf;

namespace MorMor.Model.Socket;

[ProtoContract]
[ProtoInclude(101, typeof(BasePlayerMessage))]
[ProtoInclude(102, typeof(BaseAction))]
[ProtoInclude(103, typeof(GameInitMessage))]
public class BaseMessage
{
    [ProtoMember(1)] public PostMessageType MessageType { get; set; } = PostMessageType.Action;

    [ProtoMember(2)] public string ServerName { get; set; }

    [ProtoMember(3)] public string Token { get; set; } = string.Empty;

    [JsonIgnore]
    public bool Handler { get; set; }

    [JsonIgnore]
    public TerrariaServer? TerrariaServer => MorMorAPI.Setting.GetServer(ServerName);
}
