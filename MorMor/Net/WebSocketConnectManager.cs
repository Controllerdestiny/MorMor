using Fleck;

namespace MorMor.Net;

internal class WebSocketConnectManager
{
    private readonly static Dictionary<string, IWebSocketConnection> Connect = [];

    public static void Add(string name, IWebSocketConnection connection)
    {
        Connect[name] = connection;
    }

    public static void Remove(string name)
    {
        Connect.Remove(name);
    }

    public static IWebSocketConnection? GetConnent(string name)
    {
        Connect.TryGetValue(name, out var connection);
        return connection;
    }
}
