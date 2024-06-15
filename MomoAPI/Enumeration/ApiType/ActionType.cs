using System.ComponentModel;

namespace MomoAPI.Enumeration.ApiType;

public enum ActionType
{
    #region 目前支持的 OneBotAPI
    /// <summary>
    /// 发送消息
    /// </summary>
    [Description("send_msg")]
    SendMsg,

    /// <summary>
    /// 获取cookie
    /// </summary>
    [Description("get_cookies")]
    GetCookie,

    /// <summary>
    /// 清理缓存
    /// </summary>
    [Description("clean_cache")]
    CleanChahe,

    /// <summary>
    /// 发送群消息
    /// </summary>
    [Description("send_group_msg")]
    SendGroupMsg,

    /// <summary>
    /// 发送私有消息
    /// </summary>
    [Description("send_private_msg")]
    SendPrivateMsg,

    /// <summary>
    /// 撤回消息
    /// </summary>
    [Description("delete_msg")]
    DeleteMsg,

    /// <summary>
    /// 获取登陆信息
    /// </summary>
    [Description("get_login_info")]
    GetLoginInfo,

    /// <summary>
    /// 获取群列表
    /// </summary>
    [Description("get_group_list")]
    GetGroupList,

    /// <summary>
    /// 获取群信息
    /// </summary>
    [Description("get_group_info")]
    GetGroupInfo,

    /// <summary>
    /// 获取群成员列表
    /// </summary>
    [Description("get_group_member_list")]
    GetGroupMemberList,

    /// <summary>
    /// 获取群成员信息
    /// </summary>
    [Description("get_group_member_info")]
    GetGroupMemberInfo,

    /// <summary>
    /// 获取好友员列表
    /// </summary>
    [Description("get_friend_list")]
    GetFriendList,

    /// <summary>
    /// 获取信息
    /// </summary>
    [Description("get_msg")]
    GetMsg,

    /// <summary>
    /// 点赞
    /// </summary>
    [Description("send_like")]
    Like,

    /// <summary>
    /// 入群审核处理完
    /// </summary>
    [Description("set_group_add_request")]
    GroupAddRequest,

    /// <summary>
    /// 入群审核处理完
    /// </summary>
    [Description("set_friend_add_request")]
    FriendAddRequest,

    /// <summary>
    /// 退出群组
    /// </summary>
    [Description("set_group_leave")]
    GroupLeave,

    /// <summary>
    /// 获取版本信息
    /// </summary>
    [Description("get_version_info")]
    GetVersion,

    /// <summary>
    /// 获取当前状态
    /// </summary>
    [Description("get_status")]
    GetStatus,

    /// <summary>
    /// 检查是否可以发图片
    /// </summary>
    [Description("can_send_image")]
    CanSendImage,

    /// <summary>
    /// 检查是否可以发语音
    /// </summary>
    [Description("can_send_record")]
    CanSendRecord,

    /// <summary>
    /// 移出群聊
    /// </summary>
    [Description("set_group_kick")]
    Kick,

    /// <summary>
    /// 禁言
    /// </summary>
    [Description("set_group_ban")]
    Mute,

    /// <summary>
    /// 设置群名片
    /// </summary>
    [Description("set_group_card")]
    SetCard,

    /// <summary>
    /// 设置群名
    /// </summary>
    [Description("set_group_name")]
    SetName,

    /// <summary>
    /// 全体禁言
    /// </summary>
    [Description("set_group_whole_ban")]
    MuteAll,

    /// <summary>
    /// 设置管理员
    /// </summary>
    [Description("set_group_admin")]
    SetAdmin,

    /// <summary>
    /// 获取图片
    /// </summary>
    [Description("get_image")]
    GetImage,

    /// <summary>
    /// 获取语音
    /// </summary>
    [Description("get_record")]
    GetRecord,
    #endregion

    #region LLOneBot 扩展API
    /// <summary>
    /// 设置头像 (未实现)
    /// </summary>
    [Description("set_qq_avatar")]
    SetAvatar,

    /// <summary>
    /// 获取过滤群请求 (未实现)
    /// </summary>
    [Description("get_group_ignore_add_request")]
    GroupIgnoreRequest,

    /// <summary>
    /// 下载群文件
    /// </summary>
    [Description("get_file")]
    GetFile,

    /// <summary>
    /// 表情回应
    /// </summary>
    [Description("set_msg_emoji_like")]
    EmojiLike,

    /// <summary>
    /// 转发单条消息到群
    /// </summary>
    [Description("forward_group_single_msg")]
    ForwardSingleMsgToGroup,

    /// <summary>
    /// 转发单条消息到私聊
    /// </summary>
    [Description("forward_private_single_msg")]
    ForwardSingleMsgToPrivate,

    /// <summary>
    /// 设置私聊消息已读
    /// </summary>
    [Description("mark_private_msg_as_read")]
    MarkPrivateMsgRead,

    /// <summary>
    /// 设置私聊消息已读
    /// </summary>
    [Description("mark_group_msg_as_read")]
    MarkGroupMsgRead,

    /// <summary>
    /// 获取官方BOTQQ号范围
    /// </summary>
    [Description("get_robot_uin_range")]
    GetRobotRange,

    #endregion

    #region 目前支持的CQHTPP API

    /// <summary>
    /// 获取文件列表
    /// </summary>
    [Description("get_group_file_list")]
    GetGroupFileList,

    /// <summary>
    /// 删除文件
    /// </summary>
    [Description("del_group_file")]
    DelGrpupFile,
    /// <summary>
    /// 私聊合并消息
    /// </summary>
    [Description("send_private_forward_msg")]
    SendPrivateForwardMsg,

    /// <summary>
    /// 群聊合并消息
    /// </summary>
    [Description("send_group_forward_msg")]
    SendGroupForwardMsg,

    /// <summary>
    /// 获取陌生人信息
    /// </summary>
    [Description("get_stranger_info")]
    GetStrangerInfo
    #endregion
}
