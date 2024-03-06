using MorMor.EventArgs.Sockets;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MorMor.Terraria.Server;

public class SocketServer
{
    public class State
    {
        public byte[] Buffer = new byte[1024];

        public Socket Client { get; }

        public State(Socket client)
        {
            Client = client;
        }
    }

    public delegate void SocketCallBack<in T>(T args) where T : BaseSocketArgs;

    public static event SocketCallBack<SocketDisposeArgs>? SocketDispose;

    public static event SocketCallBack<SocketConnectArgs>? SocketConnect;

    public static event SocketCallBack<SocketReceiveMessageArgs>? SocketMessage;

    private static readonly List<Socket> ClientPool = new();

    public static void Start()
    {
        var ServerEndPoint = new IPEndPoint(IPAddress.Any, MorMorAPI.Setting.SocketProt);
        var Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket.Bind(ServerEndPoint);
        Socket.Listen(20);
        Socket.BeginAccept(AcceptCallback, Socket);
    }

    public static async void Send(string msg)
    {
        var buffer = Encoding.UTF8.GetBytes(msg);
        foreach (var socket in ClientPool)
        {
            if (!socket.Poll(100, SelectMode.SelectRead) || socket.Available > 0)
            {
                await socket.SendAsync(buffer, SocketFlags.None);
            }
        }
    }

    public static void Dispose()
    {
        foreach (var client in ClientPool)
        {
            client.Disconnect(false);
            client.Dispose();
        }
    }

    private static void AcceptCallback(IAsyncResult ar)
    {
        if (ar.AsyncState is Socket client)
        {
            var socket = client.EndAccept(ar);
            var state = new State(socket);
            try
            {
                socket.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ReceiveCallback, state);
                client.BeginAccept(AcceptCallback, client);
                SocketConnect?.Invoke(new SocketConnectArgs(socket));
                ClientPool.Add(socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ClientPool.Remove(socket);
                SocketDispose?.Invoke(new SocketDisposeArgs(socket));
                client.Disconnect(false);
            }
        }
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        if (ar.AsyncState is State state)
        {
            try
            {
                state.Client.EndReceive(ar);
                SocketMessage?.Invoke(new SocketReceiveMessageArgs(state.Client, Encoding.UTF8.GetString(state.Buffer)));
                var NewState = new State(state.Client);
                if (!state.Client.Poll(100, SelectMode.SelectRead) || state.Client.Available > 0)
                    state.Client.BeginReceive(NewState.Buffer, 0, NewState.Buffer.Length, SocketFlags.None, ReceiveCallback, NewState);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                ClientPool.Remove(state.Client);
                SocketDispose?.Invoke(new SocketDisposeArgs(state.Client));
                state.Client.Disconnect(false);
            }
        }
    }
}
