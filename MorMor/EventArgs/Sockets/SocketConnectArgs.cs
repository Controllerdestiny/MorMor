using System.Net.Sockets;


namespace MorMor.EventArgs.Sockets;

public class SocketConnectArgs : BaseSocketArgs
{
    public SocketConnectArgs(Socket socket)
    {
        Client = socket;
    }
}
