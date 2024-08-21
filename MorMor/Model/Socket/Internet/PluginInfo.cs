using ProtoBuf;

namespace MorMor.Model.Socket.Internet;

[ProtoContract]
public class PluginInfo
{
    [ProtoMember(1)] public string Name { get; set; } = string.Empty;

    [ProtoMember(2)] public string Author { get; set; } = string.Empty;

    [ProtoMember(3)] public string Description { get; set; } = string.Empty;
}
