using Fleck;

namespace MorMor.EventArgs.Sockets;

public class SocketReceiveMessageArgs : BaseSocketArgs
{
    public MemoryStream Stream { get; init; }

    public SocketReceiveMessageArgs(IWebSocketConnection socket, MemoryStream stream)
    {
        Client = socket;
        Stream = stream;
    }
}
