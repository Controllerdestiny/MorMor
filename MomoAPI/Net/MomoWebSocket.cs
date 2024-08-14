using MomoAPI.Adapter;
using MomoAPI.Interface;
using MomoAPI.Net.Config;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace MomoAPI.Net;

internal class MomoWebSocket : IMomoService
{
    private WebsocketClient Client { get; set; }

    public EventAdapter Event { get; }

    public MomoWebSocket(ClientConfig config)
    {
        var ws_url = $"ws://{config.Host}:{config.Port}/";
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
                    await Event.Adapter(JObject.Parse(msg.Text));
                }
            }, token);
            token.ThrowIfCancellationRequested();
        });
        ConnectMananger.OpenConnect(Client);
        await Client.Start();
        return this;
    }

    public void Dispose()
    {
        ConnectMananger.StopService();
    }
}
