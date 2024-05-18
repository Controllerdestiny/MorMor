using MomoAPI.EventArgs;
using MorMor.Enumeration;
using MorMor.EventArgs;
using MorMor.EventArgs.Sockets;
using MorMor.Model.Socket;
using MorMor.Model.Socket.Action;
using MorMor.Model.Socket.PlayerMessage;
using MorMor.Model.Socket.ServerMessage;
using MorMor.Net;
using MorMor.Terraria.ChatCommand;
using ProtoBuf;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;

namespace MorMor.Event;

public class TerrariaMsgReceiveHandler
{
    public delegate TResult EventCallBack<in TEventArgs, out TResult>(TEventArgs args);

    public static event EventCallBack<PlayerJoinMessage, Task>? OnPlayerJoin;

    public static event EventCallBack<PlayerLeaveMessage, Task>? OnPlayerLeave;

    public static event EventCallBack<PlayerChatMessage, Task>? OnPlayerChat;

    public static event EventCallBack<PlayerCommandMessage, Task>? OnPlayerCommand;

    public static event EventCallBack<GameInitMessage, Task>? OnGameInit;

    public static event EventCallBack<BaseMessage, Task>? OnConnect;

    public static event EventCallBack<BaseMessage, Task>? OnHeartBeat;

    private static readonly Subject<(BaseAction, MemoryStream)> ApiSubject = new();

    private static readonly Dictionary<PostMessageType, EventCallBack<ServerMsgArgs, Task>> _action = new()
    {
        { PostMessageType.Action, ActionHandler },
        { PostMessageType.PlayerJoin, PlayerJoinHandler },
        { PostMessageType.PlayerLeave, PlayerLeaveHandler },
        { PostMessageType.PlayerCommand, PlayerCommandHandler },
        { PostMessageType.PlayerMessage, PlayerMessageHandler },
        { PostMessageType.GamePostInit, GamePostInitHandler },
        { PostMessageType.Connect, ConnectHandler },
        { PostMessageType.HeartBeat, HeartBeatHandler },
    };

    private static async Task PlayerMessageHandler(ServerMsgArgs args)
    {
        var data = Serializer.Deserialize<PlayerChatMessage>(args.Stream);
        OnPlayerChat?.Invoke(data);
        if (!data.Handler && data.TerrariaServer != null)
        {
            if (data.Text.Length >= data.TerrariaServer.MsgMaxLength)
                return;
            foreach (var group in data.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{data.TerrariaServer.Name}] {data.Name}: {data.Text}");
            }
        }
    }

    private static async Task HeartBeatHandler(ServerMsgArgs args)
    {
        var data = Serializer.Deserialize<BaseMessage>(args.Stream);
        OnHeartBeat?.Invoke(data);
        WebSocketConnectManager.Add(data.ServerName, args.Client);
        await Task.CompletedTask;
    }

    private static async Task ConnectHandler(ServerMsgArgs args)
    {
        var data = Serializer.Deserialize<BaseMessage>(args.Stream);
        OnConnect?.Invoke(data);
        WebSocketConnectManager.Add(data.ServerName, args.Client);
        MorMorAPI.Log.ConsoleInfo($"Terraria Server {data.ServerName} {args.Client.ConnectionInfo.ClientIpAddress} 已连接...", ConsoleColor.Green);
        await Task.CompletedTask;
    }

    private static async Task GamePostInitHandler(ServerMsgArgs args)
    {
        var data = Serializer.Deserialize<GameInitMessage>(args.Stream);
        OnGameInit?.Invoke(data);
        if (!data.Handler && data.TerrariaServer != null)
        {
            foreach (var group in data.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{data.TerrariaServer.Name}]服务器初始化已完成..");
            }
        }
    }

    private static async Task PlayerCommandHandler(ServerMsgArgs args)
    {
        var data = Serializer.Deserialize<PlayerCommandMessage>(args.Stream);
        OnPlayerCommand?.Invoke(data);
        if (!data.Handler)
        {
            await ChatCommandMananger.Hook.CommandAdapter(data);
        }
    }

    private static async Task PlayerLeaveHandler(ServerMsgArgs args)
    {
        var data = Serializer.Deserialize<PlayerLeaveMessage>(args.Stream);
        OnPlayerLeave?.Invoke(data);
        if (!data.Handler && data.TerrariaServer != null)
        {
            foreach (var group in data.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{data.TerrariaServer.Name}] {data.Name}离开服务器..");
            }
        }
    }

    private static async Task PlayerJoinHandler(ServerMsgArgs args)
    {
        var data = Serializer.Deserialize<PlayerJoinMessage>(args.Stream);
        OnPlayerJoin?.Invoke(data);
        if (!data.Handler && data.TerrariaServer != null)
        {
            foreach (var group in data.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{data.TerrariaServer.Name}] {data.Name}进入服务器..");
            }
        }
    }

    private static async Task ActionHandler(ServerMsgArgs args)
    {
        var msg = Serializer.Deserialize<BaseAction>(args.Stream);
        ApiSubject.OnNext((msg, args.Stream));
        await Task.CompletedTask;
    }

    internal static async Task<T?> GetResponse<T>(string echo, TimeSpan? timeout = null) where T : Model.Socket.Action.Response.BaseActionResponse, new()
    {
        try
        {
            if (timeout == null)
                timeout = TimeSpan.FromSeconds(15);
            var task = ApiSubject.Where(x => x.Item1.Echo == echo)
            .Select(x => x.Item2)
                .Timeout((TimeSpan)timeout)
                .Take(1)
                .ToTask();
            var stream = await task;
            stream.Position = 0;
            return Serializer.Deserialize<T>(stream);
        }
        catch (Exception ex)
        {
            return new T()
            {
                Status = false,
                Message = $"与服务器通信发生错误:{ex.Message}"
            };
        }
    }

    public static void Adapter(SocketReceiveMessageArgs args)
    {
        try
        {
            args.Stream.Position = 0;
            var baseMsg = Serializer.Deserialize<BaseMessage>(args.Stream);
            if (baseMsg.TerrariaServer != null
                && baseMsg.Token == baseMsg.TerrariaServer.Token
                && _action.TryGetValue(baseMsg.MessageType, out var action))
            {
                args.Stream.Position = 0;
                action(new()
                {
                    Client = args.Client,
                    Stream = args.Stream,
                    BaseMessage = baseMsg
                });
            }
            else
            {
                if (baseMsg.TerrariaServer == null)
                    MorMorAPI.Log.ConsoleError($"接受到{baseMsg.ServerName} 的连接请求但，在配置文件中没有找到{baseMsg.ServerName}服务器!");
                if (baseMsg.Token != baseMsg.TerrariaServer?.Token)
                    MorMorAPI.Log.ConsoleError($"{baseMsg.ServerName} 的Token 与配置文件不匹配!");
            }
        }
        catch (Exception ex)
        {
            MorMorAPI.Log.ConsoleError($"解析信息是出现错误:{ex.Message}");
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
                if (server != null && text.Length <= server.MsgMaxLength)
                {
                    if (server.ForwardGroups.Contains(args.Group.Id))
                    {
                        await server.Broadcast($"[群消息][{args.SenderInfo.Name}]: {text}", System.Drawing.Color.GreenYellow);
                    }
                }
            }
        }

    }
}
