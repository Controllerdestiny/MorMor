using System.Net.WebSockets;
using Websocket.Client;

namespace MomoAPI.Net;

internal class ConnectMananger
{
    public static WebsocketClient Client { get; private set; }

    public static void OpenConnect(WebsocketClient client)
    {
        Client = client;
    }

    public static void SendMessage(string msg)
    {
        Client.Send(msg);
    }

    public static void StopService()
    {
        Client.Stop(WebSocketCloseStatus.NormalClosure, "");
        Client.Dispose();
    }
}
