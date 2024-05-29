using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorMor.Model.Socket.Action.Receive;

[ProtoContract]
public class QueryAccountArgs : BaseAction
{
    [ProtoMember(5)] public string? Target { get; set; }
}
