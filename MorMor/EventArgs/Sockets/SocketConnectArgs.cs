using Fleck;


namespace MorMor.EventArgs.Sockets;

public class SocketConnectArgs : BaseSocketArgs
{
    public SocketConnectArgs(IWebSocketConnection socket)
    {
        Client = socket;
    }
}
