using MomoAPI.Net;

namespace MomoAPI.EventArgs;

public class BaseMomoEventArgs : System.EventArgs
{
    public OneBotAPI OneBotAPI { get;} = OneBotAPI.Instance;
}
