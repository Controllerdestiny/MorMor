using MomoAPI.Adapter;
using MomoAPI.Entities.Info;
using MomoAPI.Entities.Segment.DataModel;

namespace MomoAPI.Entities;

public class Group(long id)
{
    public long Id { get; } = id;

    public ValueTask<(ApiStatus, long)> SendMessage(MessageBody body, TimeSpan? timeout = null)
    {
        return SendMessage(Id, body, timeout);
    }

    public ValueTask<(ApiStatus, long)> Reply(MessageBody body, TimeSpan? timeout = null)
    {
        return SendMessage(Id, body, timeout);
    }

    public ValueTask<(ApiStatus, long)> SendMessage(long groupid, MessageBody body, TimeSpan? timeout = null)
    {
        return ApiAdapter.SendGroupMessage(groupid, body, timeout);
    }

    public ValueTask<(ApiStatus, long)> SendPrivateMessage(long targer, MessageBody body, TimeSpan? timeout = null)
    {
        return ApiAdapter.SendPrivateMessage(targer, body, null, timeout);
    }

    public async ValueTask Recall(long messageid)
    {
        await ApiAdapter.Recall(messageid);
    }

    public async ValueTask<(ApiStatus, List<GroupMemberInfo>)> GetMemberList()
    {
        return await ApiAdapter.GetGroupMemberList(Id);
    }

    public async ValueTask<(ApiStatus, GroupMemberInfo)> GetMemberInfo(long target)
    {
        return await ApiAdapter.GetGroupMemberInfo(Id, target);
    }

    public async ValueTask<(ApiStatus, MessageContext, Sender, long)> GetMessage(long messageid)
    {
        return await ApiAdapter.GetMessage(messageid);
    }

    public async ValueTask<ApiStatus> Mute(long userid, int duration)
    {
        return await ApiAdapter.Mute(Id, userid, duration);
    }

    public async ValueTask<ApiStatus> MuteAll(bool enable)
    {
        return await ApiAdapter.MuteAll(Id, enable);
    }

    public async ValueTask<ApiStatus> Kick(long userid, bool reject = false)
    {
        return await ApiAdapter.Kick(Id, userid, reject);
    }

    public async ValueTask<ApiStatus> SetAdmin(long userid, bool enable)
    {
        return await ApiAdapter.SetAdmin(Id, userid, enable);
    }

    public async ValueTask<ApiStatus> SetMemberCard(long userid, string card)
    {
        return await ApiAdapter.SetGroupCard(Id, userid, card);
    }

    public async ValueTask<ApiStatus> SetName(string name)
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
