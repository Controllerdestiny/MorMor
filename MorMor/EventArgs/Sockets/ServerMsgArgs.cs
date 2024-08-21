using MorMor.Model.Socket;

namespace MorMor.EventArgs.Sockets;

public class ServerMsgArgs
{
    public required BaseMessage BaseMessage { get; set; }

    public required MemoryStream Stream { get; set; }

    public required string id { get; set; }

}
