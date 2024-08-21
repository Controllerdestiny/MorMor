using MomoAPI.EventArgs;

namespace MorMor.EventArgs;

public class GroupMessageForwardArgs : System.EventArgs
{
    public required GroupMessageEventArgs GroupMessageEventArgs { get; init; }

    public string Context { get; init; } = string.Empty;

    public bool Handler { get; set; }


}
