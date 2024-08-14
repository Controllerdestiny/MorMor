namespace MorMor.EventArgs.Sockets;

public class SocketReceiveMessageArgs : BaseSocketArgs
{
    public byte[] Buffer { get; init; }

    public SocketReceiveMessageArgs(string id, byte[] buffer) : base(id)
    {
        Buffer = buffer;
    }
}
