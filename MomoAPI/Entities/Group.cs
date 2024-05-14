using MomoAPI.Adapter;
using MomoAPI.Entities.Info;
using MomoAPI.Entities.Segment.DataModel;

namespace MomoAPI.Entities;

public class Group
{
    public long Id { get; }

    public Group(long id)
    {
        Id = id;
    }

    public Task<(ApiStatus, long)> SendMessage(MessageBody body, TimeSpan? timeout = null)
    {
        return SendMessage(Id, body, timeout);
    }

    public Task<(ApiStatus, long)> Reply(MessageBody body, TimeSpan? timeout = null)
    {
        return SendMessage(Id, body, timeout);
    }

    public Task<(ApiStatus, long)> SendMessage(long groupid, MessageBody body, TimeSpan? timeout = null)
    {
        return ApiAdapter.SendGroupMessage(groupid, body, timeout);
    }

    public Task<(ApiStatus, long)> SendPrivateMessage(long targer, MessageBody body, TimeSpan? timeout = null)
    {
        return ApiAdapter.SendPrivateMessage(targer, body, null, timeout);
    }

    public async Task Recall(long messageid)
    {
        await ApiAdapter.Recall(messageid);
    }

    public async Task<(ApiStatus, List<GroupMemberInfo>)> GetMemberList()
    {
        return await ApiAdapter.GetGroupMemberList(Id);
    }

    public async Task<(ApiStatus, GroupMemberInfo)> GetMemberInfo(long target)
    {
        return await ApiAdapter.GetGroupMemberInfo(Id, target);
    }

    public async Task<(ApiStatus, MessageContext, Sender, long)> GetMessage(long messageid)
    {
        return await ApiAdapter.GetMessage(messageid);
    }

    public async Task<ApiStatus> Mute(long userid, int duration)
    {
        return await ApiAdapter.Mute(Id, userid, duration);
    }

    public async Task<ApiStatus> MuteAll(bool enable)
    {
        return await ApiAdapter.MuteAll(Id, enable);
    }

    public async Task<ApiStatus> Kick(long userid, bool reject = false)
    {
        return await ApiAdapter.Kick(Id, userid, reject);
    }

    public async Task<ApiStatus> SetAdmin(long userid, bool enable)
    {
        return await ApiAdapter.SetAdmin(Id, userid, enable);
    }

    public async Task<ApiStatus> SetMemberCard(long userid, string card)
    {
        return await ApiAdapter.SetGroupCard(Id, userid, card);
    }

    public async Task<ApiStatus> SetName(string name)
    {
        return await ApiAdapter.SetGroupName(Id, name);
    }

    internal async ValueTask<(ApiStatus apiStatus, int messageId, string forwardId)> SendForwardMsg(
        IEnumerable<CustomNode> msgList,
        TimeSpan? timeout = null)
    {
        return await ApiAdapter.SendGroupForwardMsg(Id, msgList, timeout);
    }
}
