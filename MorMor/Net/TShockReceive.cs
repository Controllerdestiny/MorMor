using System.Net.WebSockets;
using MorMor.EventArgs.Sockets;


namespace MorMor.Net;

public class TShockReceive
{

    public delegate ValueTask SocketCallBack<in T>(T args) where T : BaseSocketArgs;

    public static event SocketCallBack<SocketDisposeArgs>? SocketDispose;

    public static event SocketCallBack<SocketConnectArgs>? SocketConnect;

    public static event SocketCallBack<SocketReceiveMessageArgs>? SocketMessage;

    private static readonly CancellationToken CancellationToken = new CancellationToken();

    private static readonly WebSocketServer Server = new(MorMorAPI.Setting.SocketProt);

    public static WebSocketServer.ConnectionContext? GetConnectionContext(string id) => Server.GetConnect(id);

    public static ValueTask Send(byte[] buffer, string id) => Server.SendBytesAsync(buffer, id, CancellationToken);

    public static ValueTask Close(string id, WebSocketCloseStatus status) => Server.DisconnectAsync(id, status, CancellationToken);

    public static async ValueTask StartService()
    {
        Server.OnConnect += async (context) =>
        {
            if(SocketConnect != null)
                await SocketConnect(new(context));
        };

        Server.OnMessage += async (context, buffer) =>
        {
            if(SocketMessage != null)
                await SocketMessage(new(context, buffer));
        };

        Server.OnClose += async (id) =>
        {
           if(SocketDispose != null)
                await SocketDispose(new(id));
        };
        await Server.Start(CancellationToken);

    }
}
