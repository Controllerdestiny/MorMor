

using Newtonsoft.Json;

namespace TerrariaMap;

public class Config
{
    [JsonProperty("程序路径")]
    public string AppPath { get; set; } = string.Empty;
}
