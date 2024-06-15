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

    public async Task<IMomoService> Start()
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
        return this;
    }

    public void Dispose()
    {
        ConnectMananger.StopService();
    }
}
