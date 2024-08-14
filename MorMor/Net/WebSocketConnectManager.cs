

namespace MorMor.Net;

internal class WebSocketConnectManager
{
    private readonly static Dictionary<string, string> Connect = [];

    public static void Add(string name, string id)
    {
        Connect[name] = id;
    }

    public static void Remove(string name)
    {
        Connect.Remove(name);
    }

    public static string? GetConnentId(string name)
    {
        Connect.TryGetValue(name, out var id);
        return id;
    }

    public static WebSocketServer.ConnectionContext? GetConnent(string name)
    {
        if (Connect.TryGetValue(name, out var id))
            return TShockReceive.GetConnectionContext(id);
        return null;
    }
}
