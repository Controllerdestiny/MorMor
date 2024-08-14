namespace MorMor.EventArgs.Sockets;

public class BaseSocketArgs(string connectid)
{
    public string ConnectId { get; init; } = connectid;
}
