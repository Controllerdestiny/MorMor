using Newtonsoft.Json;

namespace MorMor.Model.Terraria.SocketMessageModel;

public class PlayerChatMessage : PlayerMessage
{
    [JsonProperty("text")]
    public string Text { get; init; }
}
