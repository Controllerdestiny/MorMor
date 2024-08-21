using MomoAPI.Converter;
using MomoAPI.Enumeration.ApiType;
using System.Text.Json.Serialization;

namespace MomoAPI.Model.API;

/// <summary>
/// API请求类
/// </summary>
public sealed class ApiRequest
{
    /// <summary>
    /// API请求类型
    /// </summary>
    [JsonPropertyName("action")]
    [JsonConverter(typeof(EnumConverter<ActionType>))]
    public ActionType ApiRequestType { get; init; }

    /// <summary>
    /// 请求标识符
    /// 会自动生成初始值不需要设置
    /// </summary>
    [JsonPropertyName("echo")]
    public Guid Echo { get; } = Guid.NewGuid();

    /// <summary>
    /// API参数对象
    /// 不需要参数时不需要设置
    /// </summary>
    [JsonPropertyName("params")]
    public dynamic ApiParams { get; init; } = new { };
}