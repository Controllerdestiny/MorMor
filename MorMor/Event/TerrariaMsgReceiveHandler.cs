using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using MomoAPI.EventArgs;
using MorMor.Enumeration;
using MorMor.EventArgs;
using MorMor.EventArgs.Sockets;
using MorMor.Model.Socket;
using MorMor.Model.Socket.Action;
using MorMor.Model.Socket.Action.Receive;
using MorMor.Model.Socket.PlayerMessage;
using MorMor.Model.Socket.ServerMessage;
using MorMor.Net;
using MorMor.TShock.ChatCommand;
using ProtoBuf;

namespace MorMor.Event;

public class TerrariaMsgReceiveHandler
{
    public delegate TResult EventCallBack<in TEventArgs, out TResult>(TEventArgs args);

    public static event EventCallBack<PlayerJoinMessage, ValueTask>? OnPlayerJoin;

    public static event EventCallBack<PlayerLeaveMessage, ValueTask>? OnPlayerLeave;

    public static event EventCallBack<PlayerChatMessage, ValueTask>? OnPlayerChat;

    public static event EventCallBack<PlayerCommandMessage, ValueTask>? OnPlayerCommand;

    public static event EventCallBack<GameInitMessage, ValueTask>? OnGameInit;

    public static event EventCallBack<BaseMessage, ValueTask>? OnConnect;

    public static event EventCallBack<BaseMessage, ValueTask>? OnHeartBeat;

    private static readonly Subject<(BaseAction, byte[])> ApiSubject = new();

    private static readonly Dictionary<PostMessageType, EventCallBack<ServerMsgArgs, ValueTask>> _action = new()
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

    private static async ValueTask PlayerMessageHandler(ServerMsgArgs args)
    {
        using var Stream = new MemoryStream(args.Buffer);
        var data = Serializer.Deserialize<PlayerChatMessage>(Stream);
        if(OnPlayerChat != null)
            await OnPlayerChat(data);
        if (!data.Handler && data.TerrariaServer != null && !data.Mute)
        {
            if (data.Text.Length >= data.TerrariaServer.MsgMaxLength)
                return;
            foreach (var group in data.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{data.TerrariaServer.Name}] {data.Name}: {data.Text}");
            }
        }
    }

    private static async ValueTask HeartBeatHandler(ServerMsgArgs args)
    {
        using var Stream = new MemoryStream(args.Buffer);
        var data = Serializer.Deserialize<BaseMessage>(Stream);
        WebSocketConnectManager.Add(data.ServerName, args.id);
        if(OnHeartBeat != null)
            await OnHeartBeat(data);
        await ValueTask.CompletedTask;
    }

    private static async ValueTask ConnectHandler(ServerMsgArgs args)
    {
        using var Stream = new MemoryStream(args.Buffer);
        var data = Serializer.Deserialize<BaseMessage>(Stream);
        WebSocketConnectManager.Add(data.ServerName, args.id);
        if(OnConnect != null) await OnConnect(data);
        Log.ConsoleInfo($"Terraria Server {data.ServerName} {args.id} 已连接...", ConsoleColor.Green);
        await data.TerrariaServer!.ReplyConnectStatus();
    }

    private static async ValueTask GamePostInitHandler(ServerMsgArgs args)
    {
        using var Stream = new MemoryStream(args.Buffer);
        var data = Serializer.Deserialize<GameInitMessage>(Stream);
        if(OnGameInit != null) await OnGameInit(data);
        if (!data.Handler && data.TerrariaServer != null)
        {
            foreach (var group in data.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{data.TerrariaServer.Name}]服务器初始化已完成..");
            }
        }
    }

    private static async ValueTask PlayerCommandHandler(ServerMsgArgs args)
    {
        using var Stream = new MemoryStream(args.Buffer);
        var data = Serializer.Deserialize<PlayerCommandMessage>(Stream);
        if(OnPlayerCommand != null) await OnPlayerCommand(data);
        if (!data.Handler)
        {
            await ChatCommandMananger.Hook.CommandAdapter(data);
        }
    }

    private static async ValueTask PlayerLeaveHandler(ServerMsgArgs args)
    {
        using var Stream = new MemoryStream(args.Buffer);
        var data = Serializer.Deserialize<PlayerLeaveMessage>(Stream);
        if(OnPlayerLeave != null) await OnPlayerLeave(data);
        if (!data.Handler && data.TerrariaServer != null)
        {
            foreach (var group in data.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{data.TerrariaServer.Name}] {data.Name}离开服务器..");
            }
        }
    }

    private static async ValueTask PlayerJoinHandler(ServerMsgArgs args)
    {
        using var Stream = new MemoryStream(args.Buffer);
        var data = Serializer.Deserialize<PlayerJoinMessage>(Stream);
        if(OnPlayerJoin != null) await OnPlayerJoin(data);
        if (!data.Handler && data.TerrariaServer != null)
        {
            foreach (var group in data.TerrariaServer.ForwardGroups)
            {
                await MomoAPI.Net.OneBotAPI.Instance.SendGroupMessage(group, $"[{data.TerrariaServer.Name}] {data.Name}进入服务器..");
            }
        }
    }

    private static async ValueTask ActionHandler(ServerMsgArgs args)
    {
        using var Stream = new MemoryStream(args.Buffer);
        var msg = Serializer.Deserialize<BaseAction>(Stream);
        ApiSubject.OnNext((msg, args.Buffer));
        await ValueTask.CompletedTask;
    }

    internal static async ValueTask<T?> GetResponse<T>(string echo, TimeSpan? timeout = null) where T : Model.Socket.Action.Response.BaseActionResponse, new()
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
            var buffer = await task;
            using var Stream = new MemoryStream(buffer);
            return Serializer.Deserialize<T>(Stream);
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

    public static async ValueTask Adapter(SocketReceiveMessageArgs args)
    {
        try
        {
            using var Stream = new MemoryStream(args.Buffer);
            var baseMsg = Serializer.Deserialize<BaseMessage>(Stream);
            if (baseMsg.TerrariaServer != null
                && baseMsg.Token == baseMsg.TerrariaServer.Token
                && _action.TryGetValue(baseMsg.MessageType, out var action))
            {
                Stream.Position = 0;
                await action(new()
                {
                    id = args.ConnectId,
                    Buffer = args.Buffer,
                    BaseMessage = baseMsg
                });
                Stream.Dispose();
            }
            else
            {
                var echo = Guid.NewGuid().ToString();
                using var ms = new MemoryStream();
                var response = new SocketConnectStatusArgs()
                {
                    ServerName = baseMsg.ServerName,
                    ActionType = ActionType.ConnectStatus,
                    Token = baseMsg.Token,
                    Echo = echo,
                };
                if (baseMsg.TerrariaServer == null)
                {
                    Log.ConsoleError($"接受到{baseMsg.ServerName} 的连接请求但，在配置文件中没有找到{baseMsg.ServerName}服务器!");
                    response.Status = SocketConnentType.ServerNull;

                }
                else if (baseMsg.Token != baseMsg.TerrariaServer?.Token)
                {
                    response.Status = SocketConnentType.VerifyError;
                    Log.ConsoleError($"{baseMsg.ServerName} 的Token 与配置文件不匹配!");
                }
                else
                {
                    Log.ConsoleError($"{baseMsg.ServerName} 未知连接错误!");
                    response.Status = SocketConnentType.Error;
                }
                Serializer.Serialize(ms, response);
                await TShockReceive.Send(ms.ToArray(), args.ConnectId);
                await TShockReceive.Close(args.ConnectId, System.Net.WebSockets.WebSocketCloseStatus.NormalClosure);
            }
        }
        catch (Exception ex)
        {
            Log.ConsoleError($"解析信息是出现错误:{ex.Message}");
        }

    }

    internal static async ValueTask GroupMessageForwardAdapter(GroupMessageEventArgs args)
    {
        if (args.MessageContext.Messages.Any(x => x.Type == MomoAPI.Enumeration.SegmentType.File))
        {
            try
            {
                var fileid = args.MessageContext.GetFileId();
                if (fileid != null)
                {
                    var (status, fileinfo) = await args.OneBotAPI.GetFile(fileid);
                    if (string.IsNullOrEmpty(fileinfo.Base64) || fileinfo.FileSize > 1024 * 1024 * 30)
                        return;
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
            }
            catch (Exception e)
            {
                await args.Reply("[GetFile] Error" + e.Message);
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
