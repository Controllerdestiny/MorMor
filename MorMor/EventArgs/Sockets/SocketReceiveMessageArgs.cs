namespace MorMor.EventArgs.Sockets;

public class SocketReceiveMessageArgs(string id, MemoryStream stream) : BaseSocketArgs(id)
{
    public MemoryStream Stream { get; init; } = stream;
}
