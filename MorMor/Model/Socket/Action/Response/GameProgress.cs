using ProtoBuf;

namespace MorMor.Model.Socket.Action.Response;

[ProtoContract]
public class GameProgress : BaseActionResponse
{
    [ProtoMember(8)] public Dictionary<string, bool> Progress { get; set; } = new();
}
