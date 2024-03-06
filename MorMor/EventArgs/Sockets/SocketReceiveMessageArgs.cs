using System.Net.Sockets;

namespace MorMor.EventArgs.Sockets;

public class SocketReceiveMessageArgs : BaseSocketArgs
{
    public string Message { get; init; }

    public SocketReceiveMessageArgs(Socket socket, string msg)
    {
        Client = socket;
        Message = msg;
    }
}
