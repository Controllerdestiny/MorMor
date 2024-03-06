using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Test;

internal class SocketClient
{
    public class State
    {
        public byte[] Buffer = new byte[2048];

        public Socket Client { get; }

        public State(Socket client)
        {
            Client = client;
        }
    }
    public Socket Client { get; set; }

    private IPAddress IPAddress { get; set; }

    private int Port { get; set; }

    private int ReConnectCount = 0;
    public SocketClient(IPAddress address, int prot)
    {
        IPAddress = address;
        Port = prot;
    }

    public void Start()
    {
        try
        {
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Client.BeginConnect(IPAddress, Port, ConnectCallBack, Client);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async void ConnectCallBack(IAsyncResult ar)
    {
        if (ar.AsyncState is Socket clint && clint.Connected)
        {
            await Console.Out.WriteLineAsync("成功连接到MorMorBOT...");
            await Task.Delay(5000);
            State state = new(clint);
            Client.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ReceiveCallBack, state);
            clint.EndConnect(ar);
            ReConnectCount = 0;
        }
        else
        {
            ReConnectCount++;
            await Console.Out.WriteLineAsync($"[{ReConnectCount}]连接已断开，五秒后尝试重新连接...");
            await Task.Delay(5000);
            Client.BeginConnect(IPAddress, Port, ConnectCallBack, Client);
        }
    }


    private void ReceiveCallBack(IAsyncResult ar)
    {
        if (ar.AsyncState is State state)
        {
            try
            {
                state.Client.EndReceive(ar);
                var NewState = new State(state.Client);
                state.Client.BeginReceive(NewState.Buffer, 0, NewState.Buffer.Length, SocketFlags.None, ReceiveCallBack, NewState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Client = null;
                Start();
            }
        }
    }

    internal void SendMsg(string msg)
    {
        if (Client.Connected)
            Client.Send(Encoding.UTF8.GetBytes(msg));
    }
}
