using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;

namespace MorMor.Terraria.Server.ApResultArgs;

public class ServerStatusArgs : BaseResultArgs
{
    [JsonProperty("name")]
    public string Name { get; init; }

    [JsonProperty("serverversion")]
    public string ServerVersion { get; init; }

    [JsonProperty("tshockversion")]
    public string TShockVersion { get; init; }

    [JsonProperty("port")]
    public int Port { get; init; }

    [JsonProperty("playercount")]
    public int PlayerCount { get; init; }

    [JsonProperty("maxplayers")]
    public int MaxPlayers { get; init; }

    [JsonProperty("uptime")]
    public string UpTime { get; init; }

    [JsonProperty("world")]
    public string World { get; init; }

    [JsonProperty("serverpassword")]
    public string ServerPassword { get; init; }
}
