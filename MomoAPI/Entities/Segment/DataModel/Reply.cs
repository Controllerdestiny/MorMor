﻿using MomoAPI.Utils;
using Newtonsoft.Json;

namespace MomoAPI.Entities.Segment.DataModel;
public record Reply : BaseMessage
{
    [JsonConverter(typeof(StringConverter))]
    [JsonProperty("id")]
    public long Uid { get; internal set; }
}
