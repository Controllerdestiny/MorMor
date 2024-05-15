using Fleck;
using MorMor.Model.Socket;

namespace MorMor.EventArgs.Sockets;

public class ServerMsgArgs
{
    public BaseMessage BaseMessage { get; set; }

    public MemoryStream Stream { get; set; }

    public IWebSocketConnection Client { get; set; }
}
