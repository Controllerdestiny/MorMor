using MomoAPI.Utils;
using MorMor.Enumeration;
using MorMor.Terraria;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace MorMor.Model.Terraria.SocketMessageModel;

public class BaseMessage
{
    //消息类型
    [JsonProperty("message_type")]
    [JsonConverter(typeof(EnumConverter))]
    public SocketMessageType MessageType { get; init; }

    [JsonProperty("server_name")]
    public string ServerName { get; init; }

    [JsonIgnore]
    public Socket Client { get; internal set; }

    [JsonIgnore]
    public TerrariaServer? TerrariaServer => MorMorAPI.Setting.GetServer(ServerName);

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
