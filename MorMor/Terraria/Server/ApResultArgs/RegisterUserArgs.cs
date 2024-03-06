using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;

namespace MorMor.Terraria.Server.ApResultArgs;

public class RegisterUserArgs : BaseResultArgs
{
    [JsonProperty("response")]
    public string Response { get; init; }
}
