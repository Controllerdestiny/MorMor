
using System.Text.Json.Serialization;

namespace MomoAPI.Model.Event;

/// <summary>
/// OneBot事件基类
/// </summary>
public abstract class BaseObApiEventArgs : System.EventArgs
{
    /// <summary>
    /// 事件发生的时间戳
    /// </summary>
    [JsonPropertyName("time")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal long Time { get; set; }

    /// <summary>
    /// 收到事件的机器人 QQ 号
    /// </summary>
    [JsonPropertyName("self_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal long SelfId { get; set; }

    /// <summary>
    /// 事件类型
    /// </summary>
    [JsonPropertyName("post_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal string PostType { get; set; } = string.Empty;
}