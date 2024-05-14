using MorMor.Model.Socket.Internet;
using ProtoBuf;
namespace MorMor.Model.Socket.Action.Response;

[ProtoContract]
public class PlayerInventory : BaseActionResponse
{
    [ProtoMember(8)] public PlayerData? PlayerData { get; set; }

}
