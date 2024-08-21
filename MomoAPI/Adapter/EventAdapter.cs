using MomoAPI.Enumeration.EventParamType;
using MomoAPI.EventArgs;
using MomoAPI.Extensions;
using MomoAPI.Model.Event.MessageEvent;
using MomoAPI.Model.Event.MetaEvent;
using MomoAPI.Model.Event.NoticeEvent;
using MomoAPI.Model.Event.RequestEvent;
using MomoAPI.Net;
using System.Text.Json.Nodes;

namespace MomoAPI.Adapter;

public class EventAdapter
{
    public delegate ValueTask EventCallBackHandler<in TEventArgs>(TEventArgs args) where TEventArgs : System.EventArgs;

    /// <summary>
    /// 群消息事件
    /// </summary>
    public event EventCallBackHandler<GroupMessageEventArgs>? OnGroupMessage;

    /// <summary>
    /// 私聊消息事件
    /// </summary>
    public event EventCallBackHandler<PrivateMessageEventArgs>? OnPrivateMessage;

    /// <summary>
    /// 群消息撤回事件
    /// </summary>
    public event EventCallBackHandler<GroupRecallEventArgs>? OnGroupRecall;

    /// <summary>
    /// 好友消息撤回事件
    /// </summary>
    public event EventCallBackHandler<FriendRecallEventArgs>? OnFriendReacll;

    /// <summary>
    /// 好友添加事件
    /// </summary>
    public event EventCallBackHandler<FriendAddEventArgs>? OnFriendAdd;

    /// <summary>
    /// 群成员变更事件
    /// </summary>
    public event EventCallBackHandler<GroupMemberChangeEventArgs>? OnGroupMemberChange;

    /// <summary>
    /// 请求入群事件
    /// </summary>
    public event EventCallBackHandler<GroupRequestAddEventArgs>? OnGroupRequestAdd;

    /// <summary>
    /// 好友请求事件
    /// </summary>
    public event EventCallBackHandler<FriendRequestAddEventArgs>? OnFriendRequestAdd;

    /// <summary>
    /// 心跳包事件
    /// </summary>
    public event EventCallBackHandler<HeartBeatEventArgs>? OnHeartBeat;

    /// <summary>
    /// 生命周期事件
    /// </summary>
    public event EventCallBackHandler<LifeCycleEventArgs>? OnLifeCycle;

    /// <summary>
    /// 群禁言事件
    /// </summary>
    public event EventCallBackHandler<GroupMuteEventArgs>? OnGroupMute;

    /// <summary>
    /// 群解除禁用事件
    /// </summary>
    public event EventCallBackHandler<GroupUnMuteEventArgs>? OnGroupUnMute;

    /// <summary>
    /// 群上传文件
    /// </summary>
    public event EventCallBackHandler<GroupUpLoadFileEventArgs>? OnGroupUpLoadFile;

    internal async ValueTask Adapter(JsonObject messageObj)
    {
        if (messageObj.TryGetPropertyValue("post_type",out var message))
        {
            OneBotAPI.Instance.BotId = messageObj["self_id"]!.GetValue<long>();
            switch (message?.ToString())
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
            if (messageObj.TryGetPropertyValue("echo", out var id) && id != null && Guid.TryParse(id.GetValue<string>(), out var echo))
            {
                ReactiveApiManager.GetResponse(echo, messageObj);
            }
        }
    }

    private async ValueTask MetaAdapter(JsonObject messageObj)
    {
        if (messageObj.TryGetPropertyValue("meta_event_type", out var type) && type != null)
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
                            if(OnLifeCycle != null) await OnLifeCycle(args);
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
                            if(OnHeartBeat != null) await OnHeartBeat(args);
                        }
                        break;
                    }
            }
        }
    }

    private async ValueTask RequestAdapter(JsonObject messageObj)
    {
        if (messageObj.TryGetPropertyValue("request_type", out var type) && type != null)
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
                        Log.ConsoleInfo($"群请求: group:{args.Group.Id} {args.User.Id} 请求入群");
                        if(OnGroupRequestAdd != null) await OnGroupRequestAdd(args);
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
                        Log.ConsoleInfo($"好友请求: {args.User.Id} 请求添加为好友");
                        if(OnFriendRequestAdd != null) await OnFriendRequestAdd(args);
                    }
                    break;
                }
            }
        }
    }

    private async ValueTask NoticeAdapter(JsonObject messageObj)
    {
        if (messageObj.TryGetPropertyValue("notice_type", out var type) && type != null)
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
                            Log.ConsoleInfo($"群消息撤回: group: {args.Group.Id} 成员`{args.MessageSender.Id}`被撤回了一条消息:{args.MessageID} 撤回人是`{args.Operator.Id}`");
                            if(OnGroupRecall != null) await OnGroupRecall(args);
                        }
                        break;
                    }
                case "friend_recall":
                    {
                        var obj =messageObj.ToObject<OneBotFriendRecallEventArgs>();
                        if (obj == null)
                            break;
                        var args = new FriendRecallEventArgs(obj);
                        if (args != null)
                        {
                            Log.ConsoleInfo($"好友撤回 {args.UID} 撤回了一条信息: {args.MessageID}");
                            if(OnFriendReacll != null) await OnFriendReacll(args);
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
                            Log.ConsoleInfo($"好友事件: {args.Sender.Id} 被添加为好友");
                            if(OnFriendAdd != null) await OnFriendAdd(args);
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
                            Log.ConsoleInfo($"群成员变动: group:{args.Group.Id} 成员({args.ChangeUser.Id}) {((args.ChangeType is MemberChangeType.Invite or MemberChangeType.Approve) ? "加入" : "离开")}群聊");
                            if(OnGroupMemberChange != null) await OnGroupMemberChange(args);
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
                                        Log.ConsoleInfo($"群禁言: group: {args.Group.Id} 成员`{args.Target.Id}`被`{args.Operator.Id}`禁用{args.Duration}秒");
                                        if(OnGroupMute != null) await OnGroupMute(args);
                                    }
                                    break;
                                }
                            case MuteType.UnMute:
                                {
                                    var args = new GroupUnMuteEventArgs(obj);
                                    if (args != null)
                                    {
                                        Log.ConsoleInfo($"群解除禁言: group: {args.Group.Id} 成员`{args.Target.Id}`被`{args.Operator.Id}`解除了禁言");
                                        if(OnGroupUnMute != null) await OnGroupUnMute(args);
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case "group_upload":
                    {
                        var obj = messageObj.ToObject<OneBotGroupUpLoadFileEventArgs>();
                        if (obj == null)
                            break;
                        var args = new GroupUpLoadFileEventArgs(obj);
                        Log.ConsoleInfo($"群文件上传事件: {args.UserId} 上传文件 {args.UpLoad.Name}");
                        if (OnGroupUpLoadFile != null) await OnGroupUpLoadFile(args);
                        break;
                    }
            }
        }
    }

    private async ValueTask MessageAdapter(JsonObject messageObj)
    {
        if (messageObj.TryGetPropertyValue("message_type", out var type) && type != null)
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
                            if (OnGroupMessage != null) await OnGroupMessage(args);
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
                            if(OnPrivateMessage != null) await OnPrivateMessage(args);
                        }
                        break;
                    }
            }
        }
    }
}
