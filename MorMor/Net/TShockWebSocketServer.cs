using Fleck;
using MorMor.EventArgs.Sockets;


namespace MorMor.Net;

public class TShockWebSocketServer
{
    private static WebSocketServer Server { get; set; }

    public delegate void SocketCallBack<in T>(T args) where T : BaseSocketArgs;

    public static event SocketCallBack<SocketDisposeArgs>? SocketDispose;

    public static event SocketCallBack<SocketConnectArgs>? SocketConnect;

    public static event SocketCallBack<SocketReceiveMessageArgs>? SocketMessage;

    public static Task StartService()
    {

        //启动服务器
        Server = new WebSocketServer($"ws://0.0.0.0:{MorMorAPI.Setting.SocketProt}/momo")
        {
            //出错后进行重启
            RestartAfterListenError = true
        };
        Server.Start(SocketEvent);
        return Task.CompletedTask;
    }

    private static void SocketEvent(IWebSocketConnection connection)
    {
        connection.OnOpen = () =>
        {
            SocketConnect?.Invoke(new SocketConnectArgs(connection));
        };

        connection.OnBinary = (Buffer) =>
        {
            SocketMessage?.Invoke(new(connection, new MemoryStream(Buffer)));
        };

        connection.OnClose = () =>
        {
            SocketDispose?.Invoke(new(connection));
        };
    }
}
