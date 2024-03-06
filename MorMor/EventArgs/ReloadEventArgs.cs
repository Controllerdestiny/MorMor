using MomoAPI.Entities;

namespace MorMor.EventArgs;

public class ReloadEventArgs : System.EventArgs
{
    public MessageBody Message { get; }

    public ReloadEventArgs()
    {
        Message = new();
    }
}
