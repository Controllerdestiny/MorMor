using System.Net.Sockets;


namespace MorMor.EventArgs.Sockets;

public class SocketDisposeArgs : BaseSocketArgs
{
    public SocketDisposeArgs(Socket socket)
    {
        Client = socket;
    }
}
