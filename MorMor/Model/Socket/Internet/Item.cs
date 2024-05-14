using ProtoBuf;

namespace MorMor.Model.Socket.Internet;


[ProtoContract]
public class Item
{
    [ProtoMember(1)] public int netID { get; set; }

    [ProtoMember(2)] public int prefix { get; set; }

    [ProtoMember(3)] public int stack { get; set; }
}
