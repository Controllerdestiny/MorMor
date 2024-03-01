using MomoAPI.Enumeration.ApiType;
using MomoAPI.Utils;
using Newtonsoft.Json;

namespace MomoAPI.Model.API;

/// <summary>
/// API请求类
/// </summary>
internal sealed class ApiRequest
{
    /// <summary>
    /// API请求类型
    /// </summary>
    [JsonProperty(PropertyName = "action")]
    [JsonConverter(typeof(EnumConverter))]
    internal ActionType ApiRequestType { get; init; }

    /// <summary>
    /// 请求标识符
    /// 会自动生成初始值不需要设置
    /// </summary>
    [JsonProperty(PropertyName = "echo")]
    internal Guid Echo { get; } = Guid.NewGuid();

    /// <summary>
    /// API参数对象
    /// 不需要参数时不需要设置
    /// </summary>
    [JsonProperty(PropertyName = "params")]
    internal dynamic ApiParams { get; init; } = new { };
}