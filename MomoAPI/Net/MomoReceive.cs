using MomoAPI.Adapter;
using MomoAPI.Interface;
using MomoAPI.Net.Config;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace MomoAPI.Net;

internal class MomoReceive : IMomoService
{
    private WebsocketClient Client { get; set; }

    public EventAdapter Event { get; }

    public MomoReceive(ClientConfig config)
    {
        var ws_url = $"ws://{config.Host}:{config.Port}/?access_token={config.AccessToken}";
        Client = new(new(ws_url));
        Event = new();
    }

    public async ValueTask<IMomoService> Start()
    {
        Client.MessageReceived.Subscribe(msg => Task.Run(async () =>
        {
            if (!string.IsNullOrEmpty(msg.Text))
            {
                await Event.Adapter(JObject.Parse(msg.Text));
            }
        }));
        ConnectMananger.OpenConnect(Client);
        await Client.Start();
        if (!Client.IsRunning || !Client.IsStarted)
        {
            throw new Exception("无法连接到WebSocket服务器");
        }
        Log.ConsoleInfo("[MorMor] 成功链接至WebSocket服务器....", ConsoleColor.Green);
        return this;
    }

    public void Dispose()
    {
        ConnectMananger.StopService();
    }
}
