using MomoAPI.EventArgs;
using MomoAPI.Utils;
using MorMor.Enumeration;
using MorMor.EventArgs;
using MorMor.EventArgs.Sockets;
using MorMor.Model.Terraria.SocketMessageModel;
using Newtonsoft.Json.Linq;

namespace MorMor.Event;

public class TerrariaMsgReceiveHandler
{
    public delegate TResult EventCallBack<in TEventArgs, out TResult>(TEventArgs args) where TEventArgs : BaseMessage;

    public static event EventCallBack<PlayerJoinMessage, Task>? OnPlayerJoin;

    public static event EventCallBack<PlayerLeaveMessage, Task>? OnPlayerLeave;

    public static event EventCallBack<PlayerChatMessage, Task>? OnPlayerChat;

    public static event EventCallBack<PlayerCommandMessage, Task>? OnPlayerCommand;

    public static event EventCallBack<GameInitMessage, Task>? OnGameInit;

    public static event EventCallBack<BaseMessage, Task>? OnConnect;

    public static event EventCallBack<BaseMessage, Task>? OnHeartBeat;

    public static void Adapter(SocketReceiveMessageArgs args)
    {
        try
        {
            var messageObj = JObject.Parse(args.Message);
            if (messageObj.TryGetValue("message_type", out var type) && type != null)
            {
                switch (EnumConverter.GetFieldByDesc<SocketMessageType>(type.ToString()))
                {
                    case SocketMessageType.PlayerLeave:
                        {
                            var msg = messageObj.ToObject<PlayerLeaveMessage>();
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnPlayerLeave?.Invoke(msg);
                                if (!msg.Handler)
                                    PlayerLeaveMessageAdapter(msg);
                            }
                            break;
                        }
                    case SocketMessageType.PlayerJoin:
                        {
                            var msg = messageObj.ToObject<PlayerJoinMessage>();
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnPlayerJoin?.Invoke(msg);
                                if (!msg.Handler)
                                    PlayerJoinMessageAdapter(msg);
                            }
                            break;
                        }
                    case SocketMessageType.PlayerMessage:
                        {
                            var msg = messageObj.ToObject<PlayerChatMessage>();
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnPlayerChat?.Invoke(msg);
                                if (!msg.Handler)
                                    PlayerMessageAdapter(msg);
                            }
                            break;
                        }
                    case SocketMessageType.PlayerCommand:
                        {
                            var msg = messageObj.ToObject<PlayerCommandMessage>();
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnPlayerCommand?.Invoke(msg);
                                if (!msg.Handler)
                                    PlayerCommandMessageAdapter(msg);
                            }
                            break;
                        }
                    case SocketMessageType.GamePostInit:
                        {
                            var msg = messageObj.ToObject<GameInitMessage>();
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnGameInit?.Invoke(msg);
                                if (!msg.Handler)
                                    GameInitAdapter(msg);
                            }
                            break;
                        }
                    case SocketMessageType.Connect:
                        {
                            var msg = messageObj.ToObject<BaseMessage>();
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnConnect?.Invoke(msg);
                                ConnectAdapter(msg);
                            }
                            break;
                        }
                    case SocketMessageType.HeartBeat:
                        {
                            var msg = messageObj.ToObject<BaseMessage>();
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnHeartBeat?.Invoke(msg);
                                HeartBeat(msg);
                            }
                            break;
                        }
                }
            }
        }
        catch
        {
            MorMorAPI.Log.ConsoleError($"{args.Client.RemoteEndPoint} 发送了一条无法解析的信息");
        }

    }

    internal static async Task GroupMessageForwardAdapter(GroupMessageEventArgs args)
    {
        var text = args.MessageContext.GetText();
        if (string.IsNullOrEmpty(text))
            return;
        var msg = new TerrariaMessageContext()
        {
            Type = SocketMessageType.PublicMsg,
            Message = $"[群消息][{args.SenderInfo.Name}]: " + args.MessageContext.GetText(),
            Color = new byte[3]
            {
                113,
                133,
                186
            }
        };
        var eventArgs = new GroupMessageForwardArgs()
        {
            Handler = false,
            GroupMessageEventArgs = args,
            Context = msg,
        };
        if (!await OperatHandler.MessageForward(eventArgs))
        {
            foreach (var server in MorMorAPI.Setting.Servers)
            {
                if (server != null)
                {
                    if (server.ForwardGroups.Contains(args.Group.Id))
                    {
                        await server.SendMessage(msg);
                    }
                }
            }
        }

    }

    private static void HeartBeat(BaseMessage msg)
    {
        var server = MorMorAPI.Setting.GetServer(msg.ServerName);
        if (server != null)
        {
            server.Client = msg.Client;
        }
    }

    private static void ConnectAdapter(BaseMessage msg)
    {
        var server = MorMorAPI.Setting.GetServer(msg.ServerName);
        if (server != null)
        {
            server.Client = msg.Client;
        }
        MorMorAPI.Log.ConsoleInfo($"Terraria Server {msg.ServerName} {msg.Client.RemoteEndPoint} 已连接...", ConsoleColor.Green);
    }

    private static async void GameInitAdapter(GameInitMessage args)
    {
        if (args.TerrariaServer != null)
        {
            foreach (var group in args.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{args.TerrariaServer.Name}]服务器初始化已完成..");
            }
        }
    }

    private static void PlayerCommandMessageAdapter(PlayerCommandMessage args)
    {

    }

    private static async void PlayerMessageAdapter(PlayerChatMessage args)
    {
        if (args.TerrariaServer != null)
        {
            foreach (var group in args.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{args.TerrariaServer.Name}] {args.Name}: {args.Text}");
            }
        }
    }

    private static async void PlayerJoinMessageAdapter(PlayerJoinMessage args)
    {
        if (args.TerrariaServer != null)
        {
            foreach (var group in args.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{args.TerrariaServer.Name}] {args.Name}进入服务器..");
            }
        }
    }

    private static async void PlayerLeaveMessageAdapter(PlayerLeaveMessage args)
    {
        if (args.TerrariaServer != null)
        {
            foreach (var group in args.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{args.TerrariaServer.Name}] {args.Name}离开服务器..");
            }
        }
    }
}
