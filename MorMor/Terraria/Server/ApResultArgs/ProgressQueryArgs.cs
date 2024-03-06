﻿using MorMor.Terraria.Server.ApiRequestParam;
using Newtonsoft.Json;

namespace MorMor.Terraria.Server.ApResultArgs;

public class ProgressQueryArgs : BaseResultArgs
{
    [JsonProperty("response")]
    public string Response { get; init; }

    [JsonProperty("data")]
    public Dictionary<string, bool> Progress { get; init; }
}
