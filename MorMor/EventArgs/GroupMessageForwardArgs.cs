using MomoAPI.EventArgs;
using MorMor.Model.Terraria.SocketMessageModel;

namespace MorMor.EventArgs;

public class GroupMessageForwardArgs : System.EventArgs
{
    public GroupMessageEventArgs GroupMessageEventArgs { get; init; }

    public TerrariaMessageContext Context { get; init; }

    public bool Handler { get; set; }


}
