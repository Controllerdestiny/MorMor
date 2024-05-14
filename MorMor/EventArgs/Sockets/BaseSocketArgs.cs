using Fleck;

namespace MorMor.EventArgs.Sockets;

public class BaseSocketArgs
{
    public IWebSocketConnection Client { get; init; }
}
