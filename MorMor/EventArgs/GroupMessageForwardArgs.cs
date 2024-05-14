using MomoAPI.EventArgs;

namespace MorMor.EventArgs;

public class GroupMessageForwardArgs : System.EventArgs
{
    public GroupMessageEventArgs GroupMessageEventArgs { get; init; }

    public string Context { get; init; }

    public bool Handler { get; set; }


}
