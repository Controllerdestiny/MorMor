using System.Net;
using Test;

var server = new SocketClient(IPAddress.Parse("127.0.0.1"), 6000);
server.Start();
while (true)
{
    var msg = Console.ReadLine();
    server.SendMsg(msg);
    server.Client.Dispose();
}



