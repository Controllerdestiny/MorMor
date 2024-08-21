using MomoAPI.Converter;
using MomoAPI.Enumeration.EventParamType;
using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Info;

public class GroupSenderInfo
{
    /// <summary>
    /// 账号
    /// </summary>
    [JsonPropertyName("user_id")]
    public long UserId { get; init; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonPropertyName("nickname")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// 群头
    /// </summary>
    [JsonPropertyName("card")]
    public string Card { get; init; } = string.Empty;

    /// <summary>
    /// 权限
    /// </summary>
    [JsonPropertyName("role")]
    [JsonConverter(typeof(EnumConverter<MemberRoleType>))]
    public MemberRoleType Role { get; init; }

    [JsonIgnore]
    public string TitleImage => $"http://q.qlogo.cn/headimg_dl?dst_uin={UserId}&spec=640&img_type=png";
}
