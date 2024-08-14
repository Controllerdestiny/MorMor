using MorMor.Model.Socket;

namespace MorMor.EventArgs.Sockets;

public class ServerMsgArgs
{
    public BaseMessage BaseMessage { get; set; }

    public byte[] Buffer { get; set; }

    public string id { get; set; }

}
