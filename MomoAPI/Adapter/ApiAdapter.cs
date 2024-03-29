﻿using MomoAPI.Converter;
using MomoAPI.Entities;
using MomoAPI.Entities.Info;
using MomoAPI.Entities.Segment.DataModel;
using MomoAPI.Enumeration.ApiType;
using MomoAPI.Enumeration.EventParamType;
using MomoAPI.Model.API;
using MomoAPI.Net;
using Newtonsoft.Json.Linq;
using Sora.Entities.Info;

namespace MomoAPI.Adapter;

internal class ApiAdapter
{
    /// <summary>
    /// 发送群消息
    /// </summary>
    /// <param name="target"></param>
    /// <param name="body"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public static async Task<(ApiStatus, long)> SendGroupMessage(long target, MessageBody body, TimeSpan? timeout = null)
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiParams = new SendMessageParams
            {
                Message = body,
                GroupId = target,
                MessageType = MessageType.Group
            },
            ApiRequestType = ActionType.SendGroupMsg
        }, timeout);
        if (status.RetCode != ApiStatusType.Ok)
            return (status, -1);
        long messageid = long.TryParse(obj?["data"]?["message_id"]?.ToString(), out var id) ? id : -1;
        return (status, messageid);
    }

    public static async Task<(ApiStatus, long)> SendPrivateMessage(long target, MessageBody body, long? group_id, TimeSpan? timeout = null)
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiParams = new SendMessageParams
            {
                Message = body,
                GroupId = group_id,
                UserId = target,
                MessageType = MessageType.Private
            },
            ApiRequestType = ActionType.SendMsg
        }, timeout);
        if (status.RetCode != ApiStatusType.Ok)
            return (status, -1);
        long messageid = long.TryParse(obj?["data"]?["message_id"]?.ToString(), out var id) ? id : -1;
        return (status, messageid);
    }

    public static async Task<ApiStatus> Recall(long messageId)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.DeleteMsg,
            ApiParams = new
            {
                message_id = messageId
            }
        });
        return status;
    }

    public static async Task<(ApiStatus, List<GroupInfo>)> GetGroupList()
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetGroupList
        });
        if (status.RetCode != ApiStatusType.Ok)
            return (status, new List<GroupInfo>());
        var groups = obj?["data"]?.ToObject<List<GroupInfo>>() ?? new List<GroupInfo>();
        return (status, groups);
    }

    public static async Task<(ApiStatus, GroupInfo)> GetGroupInfo(long groupid, bool cache)
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetGroupInfo,
            ApiParams = new
            {
                group_id = groupid,
                no_cache = !cache
            }
        });
        if (status.RetCode != ApiStatusType.Ok)
            return (status, new GroupInfo());
        var groups = obj?["data"]?.ToObject<GroupInfo>() ?? new GroupInfo();
        return (status, groups);
    }

    public static async Task<(ApiStatus, GroupMemberInfo)> GetGroupMemberInfo(long groupid, long target, bool cache = false)
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetGroupMemberInfo,
            ApiParams = new
            {
                group_id = groupid,
                user_id = target,
                no_cache = !cache
            }
        });
        if (status.RetCode != ApiStatusType.Ok)
            return (status, new GroupMemberInfo());
        var groups = obj?["data"]?.ToObject<GroupMemberInfo>() ?? new GroupMemberInfo();
        return (status, groups);
    }

    public static async Task<(ApiStatus, List<GroupMemberInfo>)> GetGroupMemberList(long groupid)
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetGroupMemberList,
            ApiParams = new
            {
                group_id = groupid
            }
        });
        if (status.RetCode != ApiStatusType.Ok)
            return (status, new List<GroupMemberInfo>());
        var groups = obj?["data"]?.ToObject<List<GroupMemberInfo>>() ?? new List<GroupMemberInfo>();
        return (status, groups);
    }

    public static async Task<(ApiStatus, List<FriendInfo>)> GetFriendList()
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetGroupMemberList,
        });
        if (status.RetCode != ApiStatusType.Ok)
            return (status, new List<FriendInfo>());
        var friends = obj?["data"]?.ToObject<List<FriendInfo>>() ?? new List<FriendInfo>();
        return (status, friends);
    }

    public static async Task<ApiStatus> SetFriendAddRequest(string flag, bool approve, string remark)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.FriendAddRequest,
            ApiParams = new { flag, approve, remark }
        });
        return status;
    }

    public static async Task<(ApiStatus, MessageContext, Sender, long)> GetMessage(long messageid)
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetMsg,
            ApiParams = new
            {
                message_id = messageid
            }
        });
        if (status.RetCode != ApiStatusType.Ok)
            return (status, new MessageContext(0, TimeSpan.Zero, null, null, -1), new Sender(-1), -1);
        List<OnebotSegment> rawMessage = obj?["data"]?["message"]?.ToObject<List<OnebotSegment>>();
        var Sender = new Sender(Convert.ToInt64(obj["data"]?["sender"]?["user_id"] ?? -1));
        var realId = Convert.ToInt64(obj["data"]?["real_id"] ?? 0);
        var body = new MessageContext(0,
            TimeSpan.FromMilliseconds(Convert.ToInt64(obj["data"]?["time"] ?? -1)),
            rawMessage.ToMessageBody() ?? new MessageBody(),
            "",
            messageid
            );
        return (status, body, Sender, realId);
    }

    public static async Task<ApiStatus> SendLike(long userid, int count)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.Like,
            ApiParams = new
            {
                user_id = userid,
                times = count
            }
        });
        return status;
    }

    public static async Task<ApiStatus> SetGroupAddRequest(string flag, bool approve, string remark)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GroupAddRequest,
            ApiParams = new { flag, approve, remark }
        });
        return status;
    }

    public static async Task<ApiStatus> SetGroupLeave(long groupid, bool dismiss = false)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GroupLeave,
            ApiParams = new
            {
                group_id = groupid,
                is_dismiss = dismiss
            }
        });
        return status;
    }

    public static async Task<ApiStatus> Kick(long groupid, long userid, bool reject = false)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.Kick,
            ApiParams = new
            {
                user_id = userid,
                group_id = groupid,
                reject_add_request = reject
            }
        });
        return status;
    }

    public static async Task<ApiStatus> Mute(long groupid, long userid, int duration)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.Mute,
            ApiParams = new
            {
                user_id = userid,
                group_id = groupid,
                duration
            }
        });
        return status;
    }

    public static async Task<ApiStatus> MuteAll(long groupid, bool enable = false)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.MuteAll,
            ApiParams = new
            {
                group_id = groupid,
                enable
            }
        });
        return status;
    }

    public static async Task<ApiStatus> SetAdmin(long groupid, long userid, bool enable)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.SetAdmin,
            ApiParams = new
            {
                user_id = userid,
                group_id = groupid,
                enable
            }
        });
        return status;
    }

    public static async Task<ApiStatus> SetGroupCard(long groupid, long userid, string card)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.SetCard,
            ApiParams = new
            {
                user_id = userid,
                group_id = groupid,
                card
            }
        });
        return status;
    }

    public static async Task<ApiStatus> SetGroupName(long groupid, string name)
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.SetName,
            ApiParams = new
            {
                group_name = name,
                group_id = groupid,
            }
        });
        return status;
    }

    public static async Task<(ApiStatus, JObject)> GetVersion()
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetVersion,
        });
        return (status, obj);
    }

    public static async Task<(ApiStatus, JObject)> GetStatus()
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetStatus,
        });
        return (status, obj);
    }

    public static async Task<(ApiStatus, bool)> CanSendImage()
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.CanSendImage,
        });
        return (status, Convert.ToBoolean(obj?["data"]?["yes"] ?? false));
    }

    public static async Task<(ApiStatus, bool)> CanSendRecord()
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.CanSendRecord,
        });
        return (status, Convert.ToBoolean(obj?["data"]?["yes"] ?? false));
    }

    internal static async ValueTask<(ApiStatus apiStatus, int messageId, string forwardId)> SendGroupForwardMsg(
        long groupId,
        IEnumerable<CustomNode> msgList,
        TimeSpan? timeout = null)
    {
        if (msgList == null)
            throw new NullReferenceException("msgList is null or empty");
        //将消息段转换为数组
        CustomNode[] customNodes = msgList as CustomNode[] ?? msgList.ToArray();
        //发送消息
        (ApiStatus apiStatus, JObject ret) =
            await ReactiveApiManager.SendApiRequest(new ApiRequest
            {
                ApiRequestType = ActionType.SendGroupForwardMsg,
                ApiParams = new
                {
                    group_id = groupId,
                    messages = customNodes.Select(node => new
                    {
                        type = "node",
                        data = node
                    }).ToList()
                }
            }, timeout);
        if (apiStatus.RetCode != ApiStatusType.Ok || ret?["data"] == null)
            return (apiStatus, -1, string.Empty);
        int msgCode = int.TryParse(ret["data"]?["message_id"]?.ToString(), out int messageCode) ? messageCode : -1;
        string fwId = ret["data"]?["forward_id"]?.ToString() ?? string.Empty;
        return (apiStatus, msgCode, fwId);
    }

    internal static async ValueTask<(ApiStatus apiStatus, int messageId)> SendPrivateForwardMsg(
        long userId,
        IEnumerable<CustomNode> msgList,
        TimeSpan? timeout = null)
    {
        if (msgList == null)
            throw new NullReferenceException("msgList is null or empty");
        //将消息段转换为数组
        CustomNode[] customNodes = msgList as CustomNode[] ?? msgList.ToArray();

        //发送消息
        (ApiStatus apiStatus, JObject ret) =
            await ReactiveApiManager.SendApiRequest(new ApiRequest
            {
                ApiRequestType = ActionType.SendPrivateForwardMsg,
                ApiParams = new
                {
                    user_id = userId,
                    messages = customNodes.Select(node => new
                    {
                        type = "node",
                        data = node
                    }).ToList()
                }
            }, timeout);
        if (apiStatus.RetCode != ApiStatusType.Ok || ret?["data"] == null)
            return (apiStatus, -1);
        int msgCode = int.TryParse(ret["data"]?["message_id"]?.ToString(), out int messageCode) ? messageCode : -1;
        return (apiStatus, msgCode);
    }

    public static async Task<(ApiStatus, UserInfo, string)> GetStrangerInfo(long target, bool cache = false)
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetStrangerInfo,
            ApiParams = new
            {
                user_id = target,
                no_cache = !cache
            }
        });
        if (status.RetCode != ApiStatusType.Ok || obj?["data"] == null)
            return (status, new UserInfo(), string.Empty);
        UserInfo info = obj["data"]?.ToObject<UserInfo>() ?? new UserInfo();
        //检查服务管理员权限

        return (status, info, obj["data"]?["qid"]?.ToString() ?? "");
    }

    public static async Task<(ApiStatus, string)> GetImage(string file)
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetImage,
            ApiParams = new
            {
                file
            }
        });
        return (status, obj["data"]?["file"]?.ToString() ?? "");
    }

    public static async Task<(ApiStatus, string)> GetRecord(string file, RecordType type = RecordType.Mp3)
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetImage,
            ApiParams = new
            {
                file,
                out_format = type
            }
        });
        return (status, obj["data"]?["file"]?.ToString() ?? "");
    }

    public static async Task<(ApiStatus, JObject)> GetCookie(string domain = "")
    {
        (ApiStatus status, JObject obj) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.GetCookie,
            ApiParams = new
            {
                domain
            }
        });
        return (status, obj);
    }

    public static async Task<ApiStatus> CleanCache()
    {
        (ApiStatus status, _) = await ReactiveApiManager.SendApiRequest(new ApiRequest()
        {
            ApiRequestType = ActionType.CleanChahe,
        });
        return status;
    }
}
