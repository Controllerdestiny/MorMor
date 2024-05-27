using MomoAPI.Adapter;
using MomoAPI.Entities;
using MomoAPI.Entities.Info;
using MomoAPI.Entities.Segment.DataModel;
using MomoAPI.Enumeration.ApiType;
using Newtonsoft.Json.Linq;

namespace MomoAPI.Net;

public class OneBotAPI
{
    public static readonly OneBotAPI Instance = new();

    public long BotId { get; set; }
    private OneBotAPI()
    {

    }

    public async Task<(ApiStatus, Entities.Info.FileInfo)> GetFile(string fileid)
    {
        return await ApiAdapter.GetFile(fileid);
    }

    public async Task<ApiStatus> EmojiLike(long msgId, string emojiid)
    {
        return await ApiAdapter.EmojiLike(msgId, emojiid);
    }

    public async Task<ApiStatus> ForwardMsgSignleGroup(long groupid, long msgId)
    {
        return await ApiAdapter.ForwardMsgSignleGroup(groupid, msgId);
    }

    public async Task<ApiStatus> ForwardMsgSignlePrivate(long userid, long msgId)
    {
        return await ApiAdapter.ForwardMsgSignlePrivate(userid, msgId);
    }

    public async Task<ApiStatus> MarkPrivateMsgAsRead(long userid)
    {
        return await ApiAdapter.MarkPrivateMsgAsRead(userid);
    }

    public async Task<ApiStatus> MarkGroupMsgAsRead(long groupid)
    {
        return await ApiAdapter.MarkGroupMsgAsRead(groupid);
    }


    public async Task<(ApiStatus, CookieInfo)> GetCookie(string domain = "qun.qq.com")
    {
        return await ApiAdapter.GetCookie(domain);
    }

    public async Task<(ApiStatus, long)> SendGroupMessage(long target, MessageBody body, TimeSpan? timeout = null)
    {
        return await ApiAdapter.SendGroupMessage(target, body, timeout);
    }

    public async Task<(ApiStatus, long)> SendPrivateMessage(long target, MessageBody body, long? group_id, TimeSpan? timeout = null)
    {
        return await SendPrivateMessage(target, body, group_id, timeout);
    }

    public async Task<ApiStatus> Recall(long messageId)
    {
        return await ApiAdapter.Recall(messageId);
    }

    public async Task<(ApiStatus, List<GroupInfo>)> GetGroupList()
    {
        return await ApiAdapter.GetGroupList();
    }

    public async Task<(ApiStatus, GroupInfo)> GetGroupInfo(long groupid, bool cache = false)
    {
        return await ApiAdapter.GetGroupInfo(groupid, cache);
    }

    public async Task<(ApiStatus, GroupMemberInfo)> GetGroupMemberInfo(long groupid, long target, bool cache = false)
    {
        return await ApiAdapter.GetGroupMemberInfo(groupid, target, cache);
    }

    public async Task<(ApiStatus, List<GroupMemberInfo>)> GetGroupMemberList(long groupid)
    {
        return await ApiAdapter.GetGroupMemberList(groupid);
    }

    public static async Task<(ApiStatus, List<FriendInfo>)> GetFriendList()
    {
        return await ApiAdapter.GetFriendList();
    }

    public async Task<ApiStatus> SetFriendAddRequest(string flag, bool approve, string remark)
    {
        return await ApiAdapter.SetFriendAddRequest(flag, approve, remark);
    }

    public async Task<(ApiStatus, MessageContext, Sender, long)> GetMessage(long messageid)
    {
        return await ApiAdapter.GetMessage(messageid);
    }

    public async Task<ApiStatus> SendLike(long userid, int count)
    {
        return await ApiAdapter.SendLike(userid, count);
    }

    public async Task<ApiStatus> SetGroupAddRequest(string flag, bool approve, string remark)
    {
        return await ApiAdapter.SetGroupAddRequest(flag, approve, remark);
    }

    public async Task<ApiStatus> SetGroupLeave(long groupid, bool dismiss = false)
    {
        return await ApiAdapter.SetGroupLeave(groupid, dismiss);
    }

    public async Task<ApiStatus> Kick(long groupid, long userid, bool reject = false)
    {
        return await ApiAdapter.Kick(groupid, userid, reject);
    }

    public async Task<ApiStatus> Mute(long groupid, long userid, int duration)
    {
        return await ApiAdapter.Mute(groupid, userid, duration);
    }

    public async Task<ApiStatus> MuteAll(long groupid, bool enable = false)
    {
        return await ApiAdapter.MuteAll(groupid, enable);
    }

    public async Task<ApiStatus> SetAdmin(long groupid, long userid, bool enable)
    {
        return await ApiAdapter.SetAdmin(groupid, userid, enable);
    }

    public async Task<ApiStatus> SetGroupCard(long groupid, long userid, string card)
    {
        return await ApiAdapter.SetGroupCard(groupid, userid, card);
    }

    public async Task<ApiStatus> SetGroupName(long groupid, string name)
    {
        return await ApiAdapter.SetGroupName(groupid, name);
    }

    public async Task<(ApiStatus, JObject)> GetVersion()
    {
        return await ApiAdapter.GetVersion();
    }

    public async Task<(ApiStatus, JObject)> GetStatus()
    {
        return await ApiAdapter.GetStatus();
    }

    public async Task<(ApiStatus, bool)> CanSendImage()
    {
        return await ApiAdapter.CanSendImage();
    }

    public async Task<(ApiStatus, bool)> CanSendRecord()
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

    public async Task<(ApiStatus, UserInfo, string)> GetStrangerInfo(long target, bool cache = false)
    {
        return await ApiAdapter.GetStrangerInfo(target, cache);
    }

    public async Task<(ApiStatus, string)> GetImage(string file)
    {
        return await ApiAdapter.GetImage(file);
    }

    public async Task<(ApiStatus, string)> GetRecord(string file, RecordType type = RecordType.Mp3)
    {
        return await ApiAdapter.GetRecord(file, type);
    }

    public async Task<ApiStatus> CleanCache()
    {
        return await ApiAdapter.CleanCache();
    }
}
