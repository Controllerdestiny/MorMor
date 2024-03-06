using MomoAPI.Entities;
using MomoAPI.EventArgs;
using MorMor.Event;
using MorMor.Model.Terraria.SocketMessageModel;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace MorMor.Terraria.Server;

public class MessageAdapter
{
    private static Dictionary<string, Socket> TShockSockets = new();
    public static void Adapter()
    {
        SocketReceiveHandler.OnGameInit += SocketReceiveHandler_OnGameInit;
        SocketReceiveHandler.OnPlayerLeave += SocketReceiveHandler_OnPlayerLeave;
        SocketReceiveHandler.OnPlayerJoin += SocketReceiveHandler_OnPlayerJoin;
        SocketReceiveHandler.OnPlayerChat += SocketReceiveHandler_OnPlayerChat;
        SocketReceiveHandler.OnConnect += SocketReceiveHandler_OnConnect;
        SocketReceiveHandler.OnHeartBeat += SocketReceiveHandler_OnHeartBeat;
    }

    private static async Task SocketReceiveHandler_OnHeartBeat(BaseMessage args)
    {
        TShockSockets[args.ServerName] = args.Client;
        await Console.Out.WriteLineAsync($"Terraria Server {args.ServerName} 心跳包");
    }

    private static async Task SocketReceiveHandler_OnConnect(BaseMessage args)
    {
        TShockSockets[args.ServerName] = args.Client;
        await Console.Out.WriteLineAsync($"Terraria Server {args.ServerName} {args.Client.RemoteEndPoint} 已连接...");
    }

    private static async Task SocketReceiveHandler_OnPlayerChat(PlayerChatMessage args)
    {
        if (args.TerrariaServer != null)
        {
            foreach (var group in args.TerrariaServer.ForwardGroups)
            {
                (ApiStatus status, _) = await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{args.TerrariaServer.Name}] {args.Name}: {args.Text}");
                await Console.Out.WriteLineAsync(status.ToString());
            }
        }
    }

    private static async Task SocketReceiveHandler_OnPlayerJoin(PlayerJoinMessage args)
    {
        if (args.TerrariaServer != null)
        {
            foreach (var group in args.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{args.TerrariaServer.Name}] {args.Name}进入服务器..");
            }
        }
    }

    private static async Task SocketReceiveHandler_OnPlayerLeave(PlayerLeaveMessage args)
    {
        if (args.TerrariaServer != null)
        {
            foreach (var group in args.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{args.TerrariaServer.Name}] {args.Name}离开服务器..");
            }
        }
    }

    private static async Task SocketReceiveHandler_OnGameInit(GameInitMessage args)
    {
        if (args.TerrariaServer != null)
        {
            foreach (var group in args.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{args.TerrariaServer.Name}]服务器初始化已完成..");
            }
        }
    }

    internal static async Task GroupMessageAdapter(GroupMessageEventArgs args)
    {
        var text = args.MessageContext.GetText();
        if (string.IsNullOrEmpty(text))
            return;
        var msg = new SendMessage()
        {
            Type = Enumeration.SocketMessageType.PluginMsg,
            Message = $"[群消息][{args.SenderInfo.Name}]: " + args.MessageContext.GetText(),
            Color = new byte[3]
            {
                113,
                133,
                186
            }
        };
        var data = JsonConvert.SerializeObject(msg);
        foreach (var cli in TShockSockets)
        {
            var server = MorMorAPI.Setting.GetServer(cli.Key);
            if (server != null)
            {
                if (server.ForwardGroups.Contains(args.Group.Id))
                {
                    if (!cli.Value.Poll(100, SelectMode.SelectRead) || cli.Value.Available > 0)
                        await cli.Value.SendAsync(Encoding.UTF8.GetBytes(data), SocketFlags.None);
                }
            }
        }
    }
}
