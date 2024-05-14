using Fleck;


namespace MorMor.EventArgs.Sockets;

public class SocketDisposeArgs : BaseSocketArgs
{
    public SocketDisposeArgs(IWebSocketConnection socket)
    {
        Client = socket;
    }
}
