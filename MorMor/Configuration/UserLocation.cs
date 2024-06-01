using MorMor.TShock.Server;
using Newtonsoft.Json;

namespace MorMor.Configuration;

public class UserLocation
{
    [JsonProperty("服务器位置")]
    public Dictionary<long, string> Location = new();

    public void Change(long id, TerrariaServer server)
    {
        Change(id, server.Name);
    }

    public void Change(long id, string Name)
    {
        Location[id] = Name;
        Save();
    }

    public bool TryGetServer(long id, long groupid, out TerrariaServer? terrariaServer)
    {
        if (Location.TryGetValue(id, out var name) && !string.IsNullOrEmpty(name))
        {
            var server = MorMorAPI.Setting.GetServer(name, groupid);
            if (server != null)
            {
                terrariaServer = server;
                return true;
            }
        }
        terrariaServer = null;
        return false;
    }

    private void Save()
    {
        Config.Write(MorMorAPI.UserLocationPath, MorMorAPI.UserLocation);
    }
}
