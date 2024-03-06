using System.Net.Sockets;

namespace MorMor.EventArgs.Sockets;

public class BaseSocketArgs
{
    public Socket Client { get; init; }
}
