using Newtonsoft.Json;

namespace MorMor.Model.Terraria.SocketMessageModel;

public class GameInitMessage : BaseMessage
{
    [JsonIgnore]
    public bool Handler { get; set; }
}
