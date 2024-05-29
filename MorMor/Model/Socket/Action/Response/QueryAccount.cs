using ProtoBuf;
using MorMor.Model.Socket.Internet;

namespace MorMor.Model.Socket.Action.Response;

[ProtoContract]
public class QueryAccount : BaseActionResponse
{
    [ProtoMember(8)] public List<Account> Accounts { get; set; } = new();
}
