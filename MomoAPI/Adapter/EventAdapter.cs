using MomoAPI.Enumeration.EventParamType;
using MomoAPI.EventArgs;
using MomoAPI.Model.Event.MessageEvent;
using MomoAPI.Model.Event.MetaEvent;
using MomoAPI.Model.Event.NoticeEvent;
using MomoAPI.Model.Event.RequestEvent;
using MomoAPI.Net;
using Newtonsoft.Json.Linq;

namespace MomoAPI.Adapter;

public class EventAdapter
{
    public delegate Task EventCallBackHandler<in TEventArgs>(TEventArgs args) where TEventArgs : System.EventArgs;

    /// <summary>
    /// 群消息事件
    /// </summary>
    public event EventCallBackHandler<GroupMessageEventArgs> OnGroupMessage;

    /// <summary>
    /// 私聊消息事件
    /// </summary>
    public event EventCallBackHandler<PrivateMessageEventArgs> OnPrivateMessage;

    /// <summary>
    /// 群消息撤回事件
    /// </summary>
    public event EventCallBackHandler<GroupRecallEventArgs> OnGroupRecall;

    /// <summary>
    /// 好友消息撤回事件
    /// </summary>
    public event EventCallBackHandler<FriendRecallEventArgs> OnFriendReacll;

    /// <summary>
    /// 好友添加事件
    /// </summary>
    public event EventCallBackHandler<FriendAddEventArgs> OnFriendAdd;

    /// <summary>
    /// 群成员变更事件
    /// </summary>
    public event EventCallBackHandler<GroupMemberChangeEventArgs> OnGroupMemberChange;

    /// <summary>
    /// 请求入群事件
    /// </summary>
    public event EventCallBackHandler<GroupRequestAddEventArgs> OnGroupRequestAdd;

    /// <summary>
    /// 好友请求事件
    /// </summary>
    public event EventCallBackHandler<FriendRequestAddEventArgs> OnFriendRequestAdd;

    /// <summary>
    /// 心跳包事件
    /// </summary>
    public event EventCallBackHandler<HeartBeatEventArgs> OnHeartBeat;

    /// <summary>
    /// 生命周期事件
    /// </summary>
    public event EventCallBackHandler<LifeCycleEventArgs> OnLifeCycle;

    /// <summary>
    /// 群禁言事件
    /// </summary>
    public event EventCallBackHandler<GroupMuteEventArgs> OnGroupMute;

    /// <summary>
    /// 群解除禁用事件
    /// </summary>
    public event EventCallBackHandler<GroupUnMuteEventArgs> OnGroupUnMute;

    internal async Task Adapter(JObject messageObj)
    {
        if (messageObj.TryGetValue("post_type", out var message) && message != null)
        {
            switch (message.ToString())
            {
                case "message":
                    await MessageAdapter(messageObj);
                    break;
                case "notice":
                    await NoticeAdapter(messageObj);
                    break;
                case "request":
                    await RequestAdapter(messageObj);
                    break;
                case "meta_event":
                    await MetaAdapter(messageObj);
                    break;
            }
        }
        else
        {
            if (messageObj.TryGetValue("echo", out var id) && id != null && Guid.TryParse(id.ToString(), out var echo))
            {
                ReactiveApiManager.GetResponse(echo, messageObj);
            }
        }
    }

    private async Task MetaAdapter(JObject messageObj)
    {
        if (messageObj.TryGetValue("meta_event_type", out var type) && type != null)
        {
            switch (type.ToString())
            {
                case "connect":
                    {
                        var obj = messageObj.ToObject<OneBotLifeCycleEventArgs>();
                        if (obj == null)
                            break;
                        var args = new LifeCycleEventArgs(obj);
                        if (args != null)
                        {
                            await OnLifeCycle(args);
                        }
                        break;
                    }
                case "heartbeat":
                    {
                        var obj = messageObj.ToObject<OnebotHeartBeatEventArgs>();
                        if (obj == null)
                            break;
                        var args = new HeartBeatEventArgs(obj);
                        if (args != null)
                        {
                            await OnHeartBeat(args);
                        }
                        break;
                    }
            }
        }
    }

    private async Task RequestAdapter(JObject messageObj)
    {
        if (messageObj.TryGetValue("notice_type", out var type) && type != null)
        {
            switch (type.ToString())
            {
                case "group":
                    {
                        var obj = messageObj.ToObject<OneBotGroupRequestAddEventArgs>();
                        if (obj == null)
                            break;
                        var args = new GroupRequestAddEventArgs(obj);
                        if (args != null)
                        {
                            await OnGroupRequestAdd(args);
                        }
                        break;
                    }
                case "friend":
                    {
                        var obj = messageObj.ToObject<OneBotFriendRequestAddEventArgs>();
                        if (obj == null)
                            break;
                        var args = new FriendRequestAddEventArgs(obj);
                        if (args != null)
                        {
                            await OnFriendRequestAdd(args);
                        }
                        break;
                    }
            }
        }
    }

    private async Task NoticeAdapter(JObject messageObj)
    {
        if (messageObj.TryGetValue("notice_type", out var type) && type != null)
        {
            switch (type.ToString())
            {
                case "group_recall":
                    {
                        var obj = messageObj.ToObject<OneBotGroupRecallEventArgs>();
                        if (obj == null)
                            break;
                        var args = new GroupRecallEventArgs(obj);
                        if (args != null)
                        {
                            await OnGroupRecall(args);
                        }
                        break;
                    }
                case "friend_recall":
                    {
                        var obj = messageObj.ToObject<OneBotFriendRecallEventArgs>();
                        if (obj == null)
                            break;
                        var args = new FriendRecallEventArgs(obj);
                        if (args != null)
                        {
                            await OnFriendReacll(args);
                        }
                        break;
                    }
                case "friend_add":
                    {
                        var obj = messageObj.ToObject<OneBotFriendAddEventArgs>();
                        if (obj == null)
                            break;
                        var args = new FriendAddEventArgs(obj);
                        if (args != null)
                        {
                            await OnFriendAdd(args);
                        }
                        break;
                    }
                case "group_increase":
                case "group_decrease":
                    {
                        var obj = messageObj.ToObject<OneBotGroupMemberChangeEventArgs>();
                        if (obj == null)
                            break;
                        var args = new GroupMemberChangeEventArgs(obj);
                        if (args != null)
                        {
                            await OnGroupMemberChange(args);
                        }
                        break;
                    }
                case "group_ban":
                    {
                        var obj = messageObj.ToObject<OneBotGroupMuteEventArgs>();
                        if (obj == null)
                            break;
                        switch (obj.OperatorType)
                        {
                            case MuteType.Mute:
                                {
                                    var args = new GroupMuteEventArgs(obj);
                                    if (args != null)
                                    {
                                        await OnGroupMute(args);
                                    }
                                    break;
                                }
                            case MuteType.UnMute:
                                {
                                    var args = new GroupUnMuteEventArgs(obj);
                                    if (args != null)
                                    {
                                        await OnGroupUnMute(args);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
            }
        }
    }

    private async Task MessageAdapter(JObject messageObj)
    {
        if (messageObj.TryGetValue("message_type", out var type) && type != null)
        {
            switch (type.ToString())
            {
                case "group":
                    {
                        var obj = messageObj.ToObject<OnebotGroupMsgEventArgs>();
                        if (obj == null)
                            break;
                        var args = new GroupMessageEventArgs(obj);
                        if (args != null)
                        {
                            await OnGroupMessage(args);
                        }
                        break;
                    }
                case "private":
                    {
                        var obj = messageObj.ToObject<OnebotPrivateMsgEventArgs>();
                        if (obj == null)
                            break;
                        var args = new PrivateMessageEventArgs(obj);
                        if (args != null)
                        {
                            await OnPrivateMessage(args);
                        }
                        break;
                    }
            }
        }
    }
}
