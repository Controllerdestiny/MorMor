using MomoAPI.Utils;
using MorMor.Enumeration;
using MorMor.EventArgs.Sockets;
using MorMor.Model.Terraria.SocketMessageModel;
using Newtonsoft.Json.Linq;

namespace MorMor.Event;

public class SocketReceiveHandler
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
                            }
                            break;
                        }
                    case SocketMessageType.PlayerMessage:
                        {
                            var msg = messageObj.ToObject<PlayerChatMessage>();
                            if (msg != null)
                            {
                                
                                OnPlayerChat?.Invoke(msg);
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
                            }
                            break;
                        }
                }
            }
        }
        catch
        {
            Console.WriteLine("接受到无法解析的信息:" + args.Message);
        }

    }
}
