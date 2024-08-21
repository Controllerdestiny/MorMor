using MomoAPI.Adapter;
using MomoAPI.Extensions;
using MomoAPI.Interface;
using MomoAPI.Net.Config;
using MomoAPI.Resolver;
using System.Text.Json;
using System.Text.Json.Nodes;
using Websocket.Client;

namespace MomoAPI.Net;

internal class MomoReceive : IMomoService
{
    private WebsocketClient Client { get; }

    public EventAdapter Event { get; }

    public MomoReceive(ClientConfig config)
    {
        var ws_url = $"ws://{config.Host}:{config.Port}/?access_token={config.AccessToken}";
        Client = new(new(ws_url));
        Event = new();
    }

    public async ValueTask<IMomoService> Start()
    {
        Client.MessageReceived.Subscribe(msg =>
        {
            var token = new CancellationToken();
            Task.Run(async () =>
            {
                if (!string.IsNullOrEmpty(msg.Text))
                {
                    var node = msg.Text.ToObject<JsonObject>();
                    if (node != null)
                        await Event.Adapter(node);
                }
            }, token);
            token.ThrowIfCancellationRequested();
        });
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
