using Newtonsoft.Json;

namespace MorMor.Model.Terraria.SocketMessageModel;

public class PlayerCommandMessage : PlayerMessage
{
    [JsonProperty("command")]
    public string Command { get; init; }
}
