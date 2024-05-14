
using MomoAPI.EventArgs;
using MomoAPI.Utils;
using MorMor.Enumeration;
using MorMor.EventArgs;
using MorMor.EventArgs.Sockets;
using MorMor.Model.Socket;
using MorMor.Model.Socket.Action;
using MorMor.Model.Socket.PlayerMessage;
using MorMor.Model.Socket.ServerMessage;
using MorMor.Terraria.ChatCommand;
using ProtoBuf;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;

namespace MorMor.Event;

public class TerrariaMsgReceiveHandler
{
    public delegate TResult EventCallBack<in TEventArgs, out TResult>(TEventArgs args) where TEventArgs : Model.Socket.BaseMessage;

    public static event EventCallBack<PlayerJoinMessage, Task>? OnPlayerJoin;

    public static event EventCallBack<PlayerLeaveMessage, Task>? OnPlayerLeave;

    public static event EventCallBack<PlayerChatMessage, Task>? OnPlayerChat;

    public static event EventCallBack<PlayerCommandMessage, Task>? OnPlayerCommand;

    public static event EventCallBack<GameInitMessage, Task>? OnGameInit;

    public static event EventCallBack<BaseMessage, Task>? OnConnect;

    public static event EventCallBack<BaseMessage, Task>? OnHeartBeat;

    private static readonly Subject<(BaseAction, MemoryStream)> ApiSubject = new();

    internal static async Task<T> GetResponse<T>(string serverName, string echo, TimeSpan? timeout = null)
    {
        if (timeout == null)
            timeout = TimeSpan.FromSeconds(15);
        var task = ApiSubject.Where(x => x.Item1.Echo == echo)
        .Select(x => x.Item2)
            .Timeout((TimeSpan)timeout)
            .Take(1)
            .ToTask()
            .RunCatch(e =>
            {
                return null;
            });
        var stream = await task;
        stream.Position = 0;
        return Serializer.Deserialize<T>(stream);

    }

    public static void Adapter(SocketReceiveMessageArgs args)
    {
        try
        {
            args.Stream.Position = 0;
            var baseMsg = Serializer.Deserialize<BaseMessage>(args.Stream);
            if (baseMsg != null)
            {
                args.Stream.Position = 0;
                switch (baseMsg.MessageType)
                {
                    case PostMessageType.Action:
                        {
                            var msg = Serializer.Deserialize<BaseAction>(args.Stream);
                            ApiSubject.OnNext((msg, args.Stream));
                            break;
                        }
                    case PostMessageType.PlayerLeave:
                        {
                            var msg = Serializer.Deserialize<PlayerLeaveMessage>(args.Stream);
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnPlayerLeave?.Invoke(msg);
                                if (!msg.Handler)
                                    PlayerLeaveMessageAdapter(msg);
                            }
                            break;
                        }
                    case PostMessageType.PlayerJoin:
                        {
                            var msg = Serializer.Deserialize<PlayerJoinMessage>(args.Stream);
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnPlayerJoin?.Invoke(msg);
                                if (!msg.Handler)
                                    PlayerJoinMessageAdapter(msg);
                            }
                            break;
                        }
                    case PostMessageType.PlayerMessage:
                        {
                            var msg = Serializer.Deserialize<PlayerChatMessage>(args.Stream);
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnPlayerChat?.Invoke(msg);
                                if (!msg.Handler)
                                    PlayerMessageAdapter(msg);
                            }
                            break;
                        }
                    case PostMessageType.PlayerCommand:
                        {
                            var msg = Serializer.Deserialize<PlayerCommandMessage>(args.Stream);
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnPlayerCommand?.Invoke(msg);
                                if (!msg.Handler)
                                    PlayerCommandMessageAdapter(msg);
                            }
                            break;
                        }
                    case PostMessageType.GamePostInit:
                        {
                            var msg = Serializer.Deserialize<GameInitMessage>(args.Stream);
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnGameInit?.Invoke(msg);
                                if (!msg.Handler)
                                    GameInitAdapter(msg);
                            }
                            break;
                        }
                    case PostMessageType.Connect:
                        {
                            var msg = Serializer.Deserialize<BaseMessage>(args.Stream);
                            if (msg != null)
                            {
                                msg.Client = args.Client;
                                OnConnect?.Invoke(msg);
                                ConnectAdapter(msg);
                            }
                            break;
                        }
                    case PostMessageType.HeartBeat:
                        {
                            var msg = Serializer.Deserialize<BaseMessage>(args.Stream);
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
        catch (Exception e)
        {
            MorMorAPI.Log.ConsoleError($"{args.Client.ConnectionInfo.ClientIpAddress} 发送了一条无法解析的信息:{e.ToString()}");
        }

    }

    internal static async Task GroupMessageForwardAdapter(GroupMessageEventArgs args)
    {
        if (args.MessageContext.Messages.Any(x => x.Type == MomoAPI.Enumeration.SegmentType.File))
        {
            try
            {
                var (status, fileinfo) = await args.OneBotAPI.GetFile(args.MessageContext.GetFileId());
                var buffer = Convert.FromBase64String(fileinfo.Base64);
                foreach (var server in MorMorAPI.Setting.Servers)
                {
                    if (server != null && server.WaitFile != null)
                    {
                        if (server.Groups.Contains(args.Group.Id))
                        {
                            server.WaitFile.TrySetResult(buffer);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
            }

        }
        var text = args.MessageContext.GetText();
        if (string.IsNullOrEmpty(text))
            return;

        var eventArgs = new GroupMessageForwardArgs()
        {
            Handler = false,
            GroupMessageEventArgs = args,
            Context = text,
        };
        if (!await OperatHandler.MessageForward(eventArgs))
        {
            foreach (var server in MorMorAPI.Setting.Servers)
            {
                if (server != null)
                {
                    if (server.ForwardGroups.Contains(args.Group.Id))
                    {
                        await server.Broadcast($"[群消息][{args.SenderInfo.Name}]: {text}", System.Drawing.Color.GreenYellow);
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
        MorMorAPI.Log.ConsoleInfo($"Terraria Server {msg.ServerName} {msg.Client.ConnectionInfo.ClientIpAddress} 已连接...", ConsoleColor.Green);
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

    private static async void PlayerCommandMessageAdapter(PlayerCommandMessage args)
    {
        await ChatCommandMananger.Hook.CommandAdapter(args);
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
