using MomoAPI.Converter;
using MomoAPI.Enumeration;
using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Extensions;
using System.Text.Json.Serialization;

namespace MomoAPI.Entities.Info;

/// <summary>
/// 群成员信息
/// </summary>
public sealed record GroupMemberInfo
{
    /// <summary>
    /// 群号
    /// </summary>
    [JsonPropertyName("group_id")]
    public long GroupId { get; init; }

    /// <summary>
    /// 成员UID
    /// </summary>
    [JsonPropertyName("user_id")]
    public long UserId { get; init; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonPropertyName("nickname")]
    public string Nick { get; init; } = string.Empty;

    /// <summary>
    /// 群名片／备注
    /// </summary>
    [JsonPropertyName("card")]
    public string Card { get; init; } = string.Empty;

    /// <summary>
    /// 性别
    /// </summary>
    [JsonPropertyName("sex")]
    [JsonConverter(typeof(EnumConverter<SexType>))]
    private SexType Sex { get; init; }


    /// <summary>
    /// 年龄
    /// </summary>
    [JsonPropertyName("age")]
    public int Age { get; init; }

    /// <summary>
    /// 地区
    /// </summary>
    [JsonPropertyName("area")]
    public string Area { get; init; } = string.Empty;

    /// <summary>
    /// 加群时间戳
    /// </summary>
    [JsonIgnore]
    public DateTime JoinTime { get; init; }

    [JsonPropertyName("join_time")]
    private long JoinTimeStamp
    {
        init => JoinTime = value.ToDateTime();
    }

    /// <summary>
    /// 最后发言时间戳
    /// </summary>
    [JsonIgnore]
    public DateTime LastSentTime { get; init; }

    [JsonPropertyName("last_sent_time")]
    private long LastSentTimeStamp
    {
        init => LastSentTime = value.ToDateTime();
    }

    /// <summary>
    /// 成员等级
    /// </summary>
    [JsonPropertyName("level")]
    public string Level { get; init; } = string.Empty;

    /// <summary>
    /// 角色(权限等级)
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MemberRoleType>))]
    [JsonPropertyName("role")]
    public MemberRoleType Role { get; init; }

    /// <summary>
    /// 是否为机器人管理员
    /// </summary>
    [JsonIgnore]
    public bool IsSuperUser { get; internal set; } = false;

    /// <summary>
    /// 是否不良记录成员
    /// </summary>
    [JsonPropertyName("unfriendly")]
    public bool Unfriendly { get; init; }

    /// <summary>
    /// 专属头衔
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// <para>专属头衔过期时间</para>
    /// <para>在<see cref="Title"/>不为空时有效</para>
    /// </summary>
    [JsonIgnore]
    public DateTime? TitleExpireTime { get; init; }


    /// <summary>
    /// 是否允许修改群名片
    /// </summary>
    [JsonPropertyName("card_changeable")]
    public bool CardChangeable { get; init; }
}