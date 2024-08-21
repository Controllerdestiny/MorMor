using MomoAPI.Adapter;
using MomoAPI.Entities;
using MomoAPI.Entities.Info;
using MomoAPI.Entities.Segment.DataModel;
using MomoAPI.Enumeration.ApiType;
using System.Text.Json.Nodes;

namespace MomoAPI.Net;

public class OneBotAPI
{
    public static readonly OneBotAPI Instance = new();

    public long BotId { get; set; }
    private OneBotAPI()
    {

    }

    public async ValueTask<(ApiStatus, Entities.Info.FileInfo)> GetFile(string fileid)
    {
        return await ApiAdapter.GetFile(fileid);
    }

    public async ValueTask<ApiStatus> EmojiLike(long msgId, string emojiid)
    {
        return await ApiAdapter.EmojiLike(msgId, emojiid);
    }

    public async ValueTask<ApiStatus> ForwardMsgSignleGroup(long groupid, long msgId)
    {
        return await ApiAdapter.ForwardMsgSignleGroup(groupid, msgId);
    }

    public async ValueTask<ApiStatus> ForwardMsgSignlePrivate(long userid, long msgId)
    {
        return await ApiAdapter.ForwardMsgSignlePrivate(userid, msgId);
    }

    public async ValueTask<ApiStatus> MarkPrivateMsgAsRead(long userid)
    {
        return await ApiAdapter.MarkPrivateMsgAsRead(userid);
    }

    public async ValueTask<ApiStatus> MarkGroupMsgAsRead(long groupid)
    {
        return await ApiAdapter.MarkGroupMsgAsRead(groupid);
    }


    public async ValueTask<(ApiStatus, CookieInfo)> GetCookie(string domain = "qun.qq.com")
    {
        return await ApiAdapter.GetCookie(domain);
    }

    public async ValueTask<(ApiStatus, long)> SendGroupMessage(long target, MessageBody body, TimeSpan? timeout = null)
    {
        return await ApiAdapter.SendGroupMessage(target, body, timeout);
    }

    public async ValueTask<(ApiStatus, long)> SendPrivateMessage(long target, MessageBody body, long? group_id, TimeSpan? timeout = null)
    {
        return await SendPrivateMessage(target, body, group_id, timeout);
    }

    public async ValueTask<ApiStatus> Recall(long messageId)
    {
        return await ApiAdapter.Recall(messageId);
    }

    public async ValueTask<(ApiStatus, List<GroupInfo>)> GetGroupList()
    {
        return await ApiAdapter.GetGroupList();
    }

    public async ValueTask<(ApiStatus, GroupInfo)> GetGroupInfo(long groupid, bool cache = false)
    {
        return await ApiAdapter.GetGroupInfo(groupid, cache);
    }

    public async ValueTask<(ApiStatus, GroupMemberInfo)> GetGroupMemberInfo(long groupid, long target, bool cache = false)
    {
        return await ApiAdapter.GetGroupMemberInfo(groupid, target, cache);
    }

    public async ValueTask<(ApiStatus, List<GroupMemberInfo>)> GetGroupMemberList(long groupid)
    {
        return await ApiAdapter.GetGroupMemberList(groupid);
    }

    public static async ValueTask<(ApiStatus, List<FriendInfo>)> GetFriendList()
    {
        return await ApiAdapter.GetFriendList();
    }

    public async ValueTask<ApiStatus> SetFriendAddRequest(string flag, bool approve, string remark)
    {
        return await ApiAdapter.SetFriendAddRequest(flag, approve, remark);
    }

    public async ValueTask<(ApiStatus, MessageContext, Sender, long)> GetMessage(long messageid)
    {
        return await ApiAdapter.GetMessage(messageid);
    }

    public async ValueTask<ApiStatus> SendLike(long userid, int count)
    {
        return await ApiAdapter.SendLike(userid, count);
    }

    public async ValueTask<ApiStatus> SetGroupAddRequest(string flag, bool approve, string remark)
    {
        return await ApiAdapter.SetGroupAddRequest(flag, approve, remark);
    }

    public async ValueTask<ApiStatus> SetGroupLeave(long groupid, bool dismiss = false)
    {
        return await ApiAdapter.SetGroupLeave(groupid, dismiss);
    }

    public async ValueTask<ApiStatus> Kick(long groupid, long userid, bool reject = false)
    {
        return await ApiAdapter.Kick(groupid, userid, reject);
    }

    public async ValueTask<ApiStatus> Mute(long groupid, long userid, int duration)
    {
        return await ApiAdapter.Mute(groupid, userid, duration);
    }

    public async ValueTask<ApiStatus> MuteAll(long groupid, bool enable = false)
    {
        return await ApiAdapter.MuteAll(groupid, enable);
    }

    public async ValueTask<ApiStatus> SetAdmin(long groupid, long userid, bool enable)
    {
        return await ApiAdapter.SetAdmin(groupid, userid, enable);
    }

    public async ValueTask<ApiStatus> SetGroupCard(long groupid, long userid, string card)
    {
        return await ApiAdapter.SetGroupCard(groupid, userid, card);
    }

    public async ValueTask<ApiStatus> SetGroupName(long groupid, string name)
    {
        return await ApiAdapter.SetGroupName(groupid, name);
    }

    public async ValueTask<(ApiStatus, JsonObject)> GetVersion()
    {
        return await ApiAdapter.GetVersion();
    }

    public async ValueTask<(ApiStatus, JsonObject)> GetStatus()
    {
        return await ApiAdapter.GetStatus();
    }

    public async ValueTask<(ApiStatus, bool)> CanSendImage()
    {
        return await ApiAdapter.CanSendImage();
    }

    public async ValueTask<(ApiStatus, bool)> CanSendRecord()
    {
        return await ApiAdapter.CanSendRecord();
    }

    public async ValueTask<(ApiStatus apiStatus, int messageId, string forwardId)> SendGroupForwardMsg(
        long groupId,
        IEnumerable<CustomNode> msgList,
        TimeSpan? timeout = null)
    {
        return await ApiAdapter.SendGroupForwardMsg(groupId, msgList, timeout);
    }

    public async ValueTask<(ApiStatus apiStatus, int messageId)> SendPrivateForwardMsg(
        long userId,
        IEnumerable<CustomNode> msgList,
        TimeSpan? timeout = null)
    {
        return await ApiAdapter.SendPrivateForwardMsg(userId, msgList, timeout);
    }

    public async ValueTask<(ApiStatus, UserInfo, string)> GetStrangerInfo(long target, bool cache = false)
    {
        return await ApiAdapter.GetStrangerInfo(target, cache);
    }

    public async ValueTask<(ApiStatus, string)> GetImage(string file)
    {
        return await ApiAdapter.GetImage(file);
    }

    public async ValueTask<(ApiStatus, List<PeerInfo>)> GetGroupFileList(long groupid)
    {
        return await ApiAdapter.GetGroupFileList(groupid);
    }

    public async ValueTask<ApiStatus> DelGroupFile(long groupid, string fileid)
    {
        return await ApiAdapter.DelGroupFile(groupid, fileid);
    }

    public async ValueTask<(ApiStatus, string)> GetRecord(string file, RecordType type = RecordType.Mp3)
    {
        return await ApiAdapter.GetRecord(file, type);
    }

    public async ValueTask<ApiStatus> CleanCache()
    {
        return await ApiAdapter.CleanCache();
    }
}
