using System.ComponentModel;

namespace MomoAPI.Enumeration.EventParamType;

public enum NoticeType
{
    /// <summary>
    /// 好友添加
    /// </summary>
    [Description("friend_add")]
    FriendAdd,

    /// <summary>
    /// 群消息撤回
    /// </summary>
    [Description("group_recall")]
    GroupRecall,

    /// <summary>
    /// 好友消息撤回
    /// </summary>
    [Description("friend_recall")]
    FriendRecall,

    /// <summary>
    /// 群成员减少
    /// </summary>
    [Description("group_decrease")]
    GroupLeave,

    /// <summary>
    /// 群成员增加
    /// </summary>
    [Description("group_increase")]
    GroupJoin
}
